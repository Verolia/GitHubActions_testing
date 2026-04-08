using MediatR;
using FluentValidation;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence.GameCatalogManagement;
using Api.Application.Dtos;

namespace Api.Features.GameCatalogManagement;

/* Description:
Takes in the details of an existing Game, updates the Game entity, and saves the changes to the database.
Tests include successful update, validation for missing or invalid fields, 
and ensuring the updated Game is correctly saved to the database.
*/

public record UpdateGameCommand(GameUpdateDto GameDTO) : IRequest<Unit>;

public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, Unit>
{
    private readonly GameCatalogManagementDbContext _db;

    public UpdateGameCommandHandler(GameCatalogManagementDbContext db)
    {
        _db = db;
    }

    public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
    {
        public UpdateGameCommandValidator()
        {
            // A rule for checking received Guid Id
            RuleFor(x => x.GameDTO.Id).NotEqual(Guid.Empty).WithMessage("Invalid game ID.");
            RuleFor(x => x.GameDTO.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.GameDTO.MaxPlayers).GreaterThan(0).WithMessage("MaxPlayers must be greater than 0.");
            RuleFor(x => x.GameDTO.PlaytimeMin).GreaterThan(0).WithMessage("PlaytimeMin must be greater than 0.");
        }
    }

    public async Task<Unit> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _db.Games.FindAsync(new object[] { request.GameDTO.Id }, cancellationToken);

        if (game == null)
        {
            throw new KeyNotFoundException($"Game with ID {request.GameDTO.Id} not found.");
        }

        // Updating the existing Game entity with the new values from the GameUpdateDto
        game.Title = request.GameDTO.Title;
        game.MaxPlayers = request.GameDTO.MaxPlayers;
        game.PlaytimeMin = request.GameDTO.PlaytimeMin;
        game.Complexity = request.GameDTO.Complexity;
        game.Description = request.GameDTO.Description;
        game.ImageUrl = request.GameDTO.ImageUrl;

        await _db.SaveChangesAsync(cancellationToken); // Saves the changes to the database

        return Unit.Value; // Returns Unit to indicate that the command has been handled successfully
    }
}