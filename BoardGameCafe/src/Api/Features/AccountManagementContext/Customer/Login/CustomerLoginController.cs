using Microsoft.AspNetCore.Mvc;
using Api.Features.AccountManagementContext.Shared.Services;
using Api.Features.AccountManagementContext.Shared.DTOs;
using Api.Features.AccountManagementContext.Enums;

namespace Api.Features.AccountManagementContext.Customer.Login;

[ApiController]
[Route("auth/customer")]
public class CustomerLoginController : ControllerBase
{
    private readonly LoginService _loginService;

    public CustomerLoginController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var credentials = new LoginCredentials(
            request.EmailAddress,
            request.Password,
            UserRole.Customer
        );

        var result = await _loginService.LoginAsync(credentials);

        if (result == null)
            return Unauthorized("Invalid email or password.");


        return Ok(new 
        {
        token = result.Token,
        role = credentials.Role.ToString()
        });
    }
}