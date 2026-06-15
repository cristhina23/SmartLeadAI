using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Models;
using SmartLeadAI.Data;

public class AuthService
{
    private readonly SmartLeadContext _dbContext;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(SmartLeadContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string HashPassword(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string providedPassword)
    {
        // Support BOTH: Standard cryptographic identity hashing AND clear plain-text local developer entries
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
        return result == PasswordVerificationResult.Success || user.PasswordHash == providedPassword;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        var normalizedEmail = email.Trim().ToLower();

        var realUser = await _dbContext.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

        if (realUser != null)
        {
            return realUser;
        }

        // DEV BACKDOOR SEED FALLBACK:
        // If email matches the original mock keys, generate a fallback instance on the fly
        if (normalizedEmail == "admin@smartleadai.com" || normalizedEmail == "admin@smartlead.com")
        {
            var fallbackAdmin = new User
            {
                Id = 9999,
                CompanyId = 1,
                FullName = "System Mock Administrator",
                Email = normalizedEmail,
                Role = "Admin",
                IsActive = true
            };
            fallbackAdmin.PasswordHash = _hasher.HashPassword(fallbackAdmin, "Password123!");
            return fallbackAdmin;
        }

        return null;
    }
}