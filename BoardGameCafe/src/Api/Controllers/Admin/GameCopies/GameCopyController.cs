using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features;
using Api.Features.GameCopies;

namespace Api.Controllers;

[ApiController]
[Route("api/games/{GameId:guid}/copies")]
public class GameCopyController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameCopyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid gameId, CancellationToken ct)
    {
        var command = new AddGameCopyCommand(gameId);
        await _mediator.Send(command, ct);
        return Created("", null);
    }

    [HttpDelete("{gameCopyId:guid}")]
    public async Task<IActionResult> Remove(Guid GameId, Guid GameCopyId, CancellationToken ct)
    {
        var command = new RemoveGameCopyCommand(GameId, GameCopyId);
        await _mediator.Send(command, ct);
        return NoContent();
    }
}