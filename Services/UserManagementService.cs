// SmartLeadAI/Services/AuthService.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


public class AuthService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
        return result == PasswordVerificationResult.Success;
    }

    /// <summary>
    /// MOCK METHOD: Simulates a database user lookup.
    /// This allows the project to compile and run until EF Core / Database logic is added.
    /// </summary>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        // Simulate database latency (e.g., 100ms)
        await Task.Delay(100);

        // Normalize email to prevent casing mismatches
        if (string.IsNullOrWhiteSpace(email)) return null;
        var normalizedEmail = email.Trim().ToLower();

        // Create a fake test user
        if (normalizedEmail == "admin@smartleadai.com" || normalizedEmail == "admin@smartlead.com")
        {
            var fakeUser = new User
            {
                Id = 1,
                Email = normalizedEmail,
                Role = "Admin"
            };
            
            // Hash the password "Password123!" for this fake user instance
            fakeUser.PasswordHash = HashPassword(fakeUser, "Password123!");
            
            return fakeUser;
        }
        
        if (normalizedEmail == "user@smartleadai.com" || normalizedEmail == "user@smartlead.com")
        {
            var fakeUser = new User
            {
                Id = 2,
                Email = normalizedEmail,
                Role = "User"
            };
            fakeUser.PasswordHash = HashPassword(fakeUser, "Test456!");
            return fakeUser;
        }

        // Return null if the email doesn't match our fake records (Simulates "User Not Found")
        return null;
    }
}