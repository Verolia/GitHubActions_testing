using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features;

namespace Api.Controllers;

[ApiController]
[Route("games")]
public class GamesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GamesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var games = await _mediator.Send(new GetAllGamesQuery(), ct);
        return Ok(games);
    }
}