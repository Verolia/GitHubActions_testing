using Api.Infrastructure.Persistence;
using Api.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly MainDbContext _db;
    private readonly JwtTokenService _jwt;

    public AuthController(MainDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress);

        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized("Invalid credentials");

        var token = _jwt.GenerateToken(user);

        return Ok(new { token, role = user.Role.ToString() });
    }
}

public record LoginRequest(string EmailAddress, string Password);