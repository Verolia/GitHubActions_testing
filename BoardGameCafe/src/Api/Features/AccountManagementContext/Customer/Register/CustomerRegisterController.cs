using Microsoft.AspNetCore.Mvc;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Features.AccountManagementContext.Shared.DTOs;
using Api.Features.AccountManagementContext.Entities;
using Api.Features.AccountManagementContext.Enums;
using Api.Features.AccountManagementContext.Shared.Services;

namespace Api.Features.AccountManagementContext.Customer.Register;

[ApiController]
[Route("auth/customer")]
public class CustomerRegisterController : ControllerBase
{
    private readonly AccountManagementDbContext _db;
    private readonly LoginService _loginService;

    public CustomerRegisterController(AccountManagementDbContext db, LoginService loginService)
    {
        _db = db;
        _loginService = loginService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        // Create the new customer
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            EmailAddress = request.EmailAddress,
            PasswordHash = request.Password,
            Role = UserRole.Customer,
            IsActive = true 
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Use LoginService to generate token
        var credentials = new LoginCredentials(request.EmailAddress, request.Password, UserRole.Customer);
        var loginResult = await _loginService.LoginAsync(credentials);

        if (loginResult == null)
        {
            return BadRequest("User was created but login failed.");
        }

        return Ok(new
        {
            token = loginResult.Token,
            role = loginResult.Role
        });
    }
}