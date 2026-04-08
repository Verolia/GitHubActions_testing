using MediatR;
using FluentValidation;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence;

namespace Api.Features.GameCopies;

public record AddGameCopyCommand(Guid GameId) : IRequest<GameCopy>; // Takes inn the GameId of the Game this GameCopy is associated with, and returns the created GameCopy

public class AddGameCopyCommandHandler : IRequestHandler<AddGameCopyCommand, GameCopy>
{
    private readonly MainDbContext _db; // Injecting the DbContext to access the database

    public AddGameCopyCommandHandler(MainDbContext db) // Constructor to receive the DbContext through dependency injection
    {
        _db = db;
    }

    public class AddGameCopyCommandValidator : AbstractValidator<AddGameCopyCommand>
    {
        public AddGameCopyCommandValidator()
        {
            RuleFor(x => x.GameId).NotEmpty().WithMessage("GameId is required.");
        }
    }

    public async Task<GameCopy> Handle(AddGameCopyCommand request, CancellationToken cancellationToken)
    {
        var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken); // Checks if the Game with the provided GameId exists in the database
        if (game == null) throw new KeyNotFoundException($"Game with ID {request.GameId} was not found.");

        var gameCopy = new GameCopy
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            
        };
        // Set default values for status and condition
        gameCopy.SetAvailable(); 
        gameCopy.MarkConditionGood(); 

        _db.GameCopies.Add(gameCopy); // Adds the new GameCopy to the database context
        await _db.SaveChangesAsync(cancellationToken); // Saves the changes to the database, which will insert the new GameCopy record

        return gameCopy;
    }
}