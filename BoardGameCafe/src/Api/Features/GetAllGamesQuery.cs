using MediatR;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence.GameCatalogManagement;
using Api.Application.Dtos;

namespace Api.Features;
public record GetAllGamesQuery() : IRequest<List<GameDto>>;
  
// Handler for GetAllGamesQuery
public sealed class GetAllGamesHandler : IRequestHandler<GetAllGamesQuery, List<GameDto>>
{

    // “These two lines below let your feature code use the database. 
    // They don’t create the database connection — 
    // they just take the already-prepared GameCatalogManagementDbContext and 
    // store it in _db so you can run queries with it.”
    private readonly GameCatalogManagementDbContext _db;
    public GetAllGamesHandler(GameCatalogManagementDbContext db) => _db = db;
  
    public async Task<List<GameDto>> Handle(GetAllGamesQuery q, CancellationToken ct)
    {
        // This is how you run queries with the database.
        return await _db.Games
            .OrderBy(g => g.Id)
            .Select(g => new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                MaxPlayers = g.MaxPlayers,
                PlaytimeMin = g.PlaytimeMin,
                Complexity = g.Complexity,
                Description = g.Description,
                ImageUrl = g.ImageUrl
            })
            .ToListAsync(ct);
    }

}