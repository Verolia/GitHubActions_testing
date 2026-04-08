using Api.Infrastructure.Persistence.AccountManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Features.AccountManagementContext.Entities;
using Api.Features.AccountManagementContext.Enums;

namespace Api.Features.AccountManagementContext.Admin.Register;

[ApiController]
[Route("admin/users")]
[Authorize(Roles = "Admin")]
public class AdminCreateUserController : ControllerBase
{
    private readonly AccountManagementDbContext _db;

    public AdminCreateUserController(AccountManagementDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        // Prevent duplicate emails
        var exists = _db.Users.Any(u => u.EmailAddress == request.EmailAddress);
        if (exists)
            return BadRequest("User already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            EmailAddress = request.EmailAddress,
            PasswordHash = request.Password, // TODO: hash this
            Role = request.Role,

            //Rules
            IsActive = true, 
            ForcePasswordChange = request.Role == UserRole.Admin 
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            user.Id,
            user.EmailAddress,
            role = user.Role.ToString()
        });
    }
}

public record CreateUserRequest(
    string Name,
    string EmailAddress,
    string Password,
    UserRole Role
);