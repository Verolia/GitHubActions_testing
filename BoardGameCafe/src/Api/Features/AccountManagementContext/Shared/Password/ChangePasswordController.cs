using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Features.AccountManagementContext.Shared.DTOs;
using Api.Features.AccountManagementContext.Shared.Services;

namespace Api.Features.AccountManagementContext.Shared.Password;

[ApiController]
[Route("account")]
[Authorize] 
public class ChangePasswordController : ControllerBase
{
    private readonly AccountManagementDbContext _dbContext;
    private readonly PasswordService _passwordService;

    public ChangePasswordController(AccountManagementDbContext dbContext, PasswordService passwordService)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var emailFromToken = User.Identity?.Name; 
        if (emailFromToken != request.EmailAddress)
        {
            return Forbid("You can only change your own password.");
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress);
        if (user == null)
            return NotFound("User not found.");

        if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            return Unauthorized("Current password is incorrect.");

        user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
        user.ForcePasswordChange = false; 
        await _dbContext.SaveChangesAsync();

        return Ok("Password changed successfully.");
    }
}