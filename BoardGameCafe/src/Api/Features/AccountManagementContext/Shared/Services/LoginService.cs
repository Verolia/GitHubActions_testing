using Api.Infrastructure.Auth;
using Api.Infrastructure.Persistence.AccountManagement;
using Microsoft.EntityFrameworkCore;
using Api.Features.AccountManagementContext.Entities;
using Api.Features.AccountManagementContext.Enums;
using Api.Features.AccountManagementContext.Shared.DTOs;

namespace Api.Features.AccountManagementContext.Shared.Services;

public class LoginService
{
    private readonly AccountManagementDbContext _db;
    private readonly JwtTokenService _jwt;

    public LoginService(AccountManagementDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<LoginResponse?> LoginAsync(LoginCredentials credentials)
    {
        // Fetch user by email and role
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.EmailAddress == credentials.EmailAddress && u.Role == credentials.Role);

        if (user == null)
            return null;

        if (!user.IsActive)
            throw new Exception("Account not activated");

        // Validate password (hash check TODO later)
        if (user.PasswordHash != credentials.Password)
            return null;

        // Create TokenRequest from User entity
        var tokenRequest = new TokenRequest(
            user.Id,
            user.EmailAddress,
            user.Role.ToString()
        );

        // Generate JWT using TokenRequest
        var token = _jwt.GenerateToken(tokenRequest);

        // Return response
        return new LoginResponse(
            token,
            user.Role.ToString(),
            user.ForcePasswordChange
        );
    }
}

// Input DTO from controller
public record LoginCredentials(string EmailAddress, string Password, UserRole Role);

// Output DTO to controller
public record LoginResponse(string Token, string Role, bool ForcePasswordChange);