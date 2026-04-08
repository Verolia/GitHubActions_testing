using MediatR;
using FluentValidation;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Api.Application.Dtos;
using Api.Infrastructure.Persistence.GameCatalogManagement;

namespace Api.Features.GameCatalogManagement;

/* Description:
Takes in the details of a new Game, creates a new Game entity, and saves it to the database.
Tests include successful creation, validation for missing or invalid fields, and 
ensuring the created Game is correctly saved to the database with an assigned Id.
*/


public record CreateGameCommand(GameCreateDto GameDTO) : IRequest<Game>;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Game>
{
    private readonly GameCatalogManagementDbContext _db; // Injecting the DbContext to access the database

    public CreateGameCommandHandler(GameCatalogManagementDbContext db) // Constructor to receive the DbContext through dependency injection
    {
        _db = db;
    }

    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameCommandValidator()
        {
            RuleFor(x => x.GameDTO.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.GameDTO.MaxPlayers).GreaterThan(0).WithMessage("MaxPlayers must be greater than 0.");
            RuleFor(x => x.GameDTO.PlaytimeMin).GreaterThan(0).WithMessage("PlaytimeMin must be greater than 0.");
        }
    }

    public async Task<Game> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        // Translating the incoming GameCreateDto into a Game entity that can be saved to the database
        var game = new Game
        {
            Title = request.GameDTO.Title,
            MaxPlayers = request.GameDTO.MaxPlayers,
            PlaytimeMin = request.GameDTO.PlaytimeMin,
            Complexity = request.GameDTO.Complexity,
            Description = request.GameDTO.Description,
            ImageUrl = request.GameDTO.ImageUrl
        };

        _db.Games.Add(game); // Adds the new Game to the database context
        await _db.SaveChangesAsync(cancellationToken); // Saves the changes to the database

        return game;
    }
}