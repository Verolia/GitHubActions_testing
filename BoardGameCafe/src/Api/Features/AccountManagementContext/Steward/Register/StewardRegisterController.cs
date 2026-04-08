using Microsoft.AspNetCore.Mvc;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Features.AccountManagementContext.Shared.DTOs;
using Api.Features.AccountManagementContext.Entities;
using Api.Features.AccountManagementContext.Enums;

namespace Api.Features.AccountManagementContext.Steward.Register;

[ApiController]
[Route("auth/steward")]
public class StewardRegisterController : ControllerBase
{
    private readonly AccountManagementDbContext _db;

    public StewardRegisterController(AccountManagementDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            EmailAddress = request.EmailAddress,
            PasswordHash = request.Password,
            Role = UserRole.Steward,
            IsActive = false // Must be approved by admin
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok("Steward account created. Awaiting admin approval.");
    }
}