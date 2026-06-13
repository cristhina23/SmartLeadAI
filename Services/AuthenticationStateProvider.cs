// SmartLeadAI/Security/CustomAuthStateProvider.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using SmartLeadAI.Models;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private ClaimsPrincipal _currentUser;

    public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _currentUser = _anonymous;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContextUser = _httpContextAccessor.HttpContext?.User;
        if (httpContextUser?.Identity?.IsAuthenticated == true)
        {
            _currentUser = httpContextUser;
        }

        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public void MarkUserAsAuthenticated(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        _currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public void MarkUserAsLoggedOut()
    {
        _currentUser = _anonymous;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
}