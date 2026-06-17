using SmartLeadAI.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Data;
using SmartLeadAI.Models;
using SmartLeadAI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SmartLeadAI.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri)
});
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddDbContextFactory<SmartLeadContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddScoped<SmartLeadContext>(p => 
    p.GetRequiredService<IDbContextFactory<SmartLeadContext>>().CreateDbContext());

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<InteractionService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddHttpClient<GeminiService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SmartLeadContext>();

    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/signin", async (
    [Microsoft.AspNetCore.Mvc.FromForm] LoginModel login,
    [Microsoft.AspNetCore.Mvc.FromForm] string? returnUrl,
    AuthService authService,
    IHttpContextAccessor httpContextAccessor) =>
{
    var user = await authService.GetUserByEmailAsync(login.Email);
    
    if (user == null || !authService.VerifyPassword(user, login.Password))
    {
        string errorQueryValue = Uri.EscapeDataString("Invalid email or password combination.");
        return Results.Redirect($"/login?error={errorQueryValue}");
    }

    var claims = new[] {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    var httpContext = httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
        return Results.Problem("Unable to establish the current HTTP context.");
    }

    await httpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        new AuthenticationProperties
        {
            IsPersistent = login.RememberMe,
            AllowRefresh = true
        });

    var redirectUrl = string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/"
        ? "/dashboard"
        : returnUrl;

    return Results.Redirect(redirectUrl);
})
.DisableAntiforgery();

app.MapPost("/register", async (
    [Microsoft.AspNetCore.Mvc.FromForm] CompanyRegistrationModel registration,
    SmartLeadContext dbContext,
    AuthService authService,
    HttpContext httpContext) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(registration);

    if (!Validator.TryValidateObject(registration, validationContext, validationResults, validateAllProperties: true))
    {
        var message = validationResults.FirstOrDefault()?.ErrorMessage ?? "Please check the registration form.";
        return Results.Redirect($"/register?error={Uri.EscapeDataString(message)}");
    }

    var normalizedEmail = registration.AdminEmail.Trim().ToLower();
    var emailExists = await dbContext.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);

    if (emailExists)
    {
        return Results.Redirect("/register?error=An%20account%20with%20that%20email%20already%20exists.");
    }

    var now = DateTime.UtcNow;
    var company = new Company
    {
        Name = registration.CompanyName.Trim(),
        Email = registration.CompanyEmail.Trim(),
        Phone = registration.CompanyPhone.Trim(),
        CreatedAt = now
    };

    var admin = new User
    {
        FullName = registration.AdminName.Trim(),
        Email = normalizedEmail,
        Role = "Admin",
        IsActive = true,
        CreatedAt = now,
        ActivatedAt = now
    };

    admin.PasswordHash = authService.HashPassword(admin, registration.AdminPassword);
    company.Users.Add(admin);

    dbContext.Companies.Add(company);
    await dbContext.SaveChangesAsync();

    var claims = new[] {
        new Claim(ClaimTypes.Name, admin.Email),
        new Claim(ClaimTypes.Email, admin.Email),
        new Claim(ClaimTypes.Role, admin.Role)
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    await httpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = true
        });

    return Results.Redirect("/dashboard");
})
.DisableAntiforgery();

app.MapPost("/signout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
