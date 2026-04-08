using Microsoft.AspNetCore.Mvc;
using MediatR;
using Api.Features.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Api.Infrastructure.Auth;


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtTokenService _jwt;

    public UserController(IMediator mediator, JwtTokenService jwt)
    {
        _mediator = mediator;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Return all validation errors as a JSON array
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            return BadRequest(new { errors });
        }

        try
        {
            var user = await _mediator.Send(command);
            var token = _jwt.GenerateToken(user);

            return Ok(new
            {
                token,
                role = user.Role.ToString(),
                user = new { user.Id, user.Name, user.EmailAddress }
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { errors = new[] { ex.Message } });
        }
    }


    // TODO: Check that AUTH is working/implemented right, only allow deleting if user is admin or deleting their own account
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized();

        var loggedInUserId = Guid.Parse(userIdClaim.Value);

        if (loggedInUserId != id)
            return Forbid();

        await _mediator.Send(new DeleteUserCommand(id));

        return NoContent();
    }
}
