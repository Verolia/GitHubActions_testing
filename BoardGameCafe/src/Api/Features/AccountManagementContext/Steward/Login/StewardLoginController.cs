using Microsoft.AspNetCore.Mvc;
using Api.Features.AccountManagementContext.Shared.Services;
using Api.Features.AccountManagementContext.Shared.DTOs;
using Api.Features.AccountManagementContext.Enums;

namespace Api.Features.AccountManagementContext.Steward.Login;

[ApiController]
[Route("auth/steward")]
public class StewardLoginController : ControllerBase
{
    private readonly LoginService _loginService;

    public StewardLoginController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var credentials = new LoginCredentials(
            request.EmailAddress,
            request.Password,
            UserRole.Steward
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