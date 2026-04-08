using MediatR;
using FluentValidation;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence.GameCatalogManagement;

namespace Api.Features.GameCopies;

public record RemoveGameCopyCommand(Guid GameId, Guid GameCopyId) : IRequest<Unit>; // Takes in the GameCopyId of the GameCopy to be removed, and returns nothing

public class RemoveGameCopyCommandHandler : IRequestHandler<RemoveGameCopyCommand, Unit>
{
    private readonly GameCatalogManagementDbContext _db; // Injecting the DbContext to access the database

    public RemoveGameCopyCommandHandler(GameCatalogManagementDbContext db) // Constructor to receive the DbContext through dependency injection
    {
        _db = db;
    }

    public class RemoveGameCopyCommandValidator : AbstractValidator<RemoveGameCopyCommand>
    {
        public RemoveGameCopyCommandValidator()
        {
            RuleFor(x => x.GameCopyId).NotEmpty().WithMessage("GameCopyId is required.");
            RuleFor(x => x.GameId).NotEmpty().WithMessage("GameId is required.");
        }
    }

    // The handle below checks if keys GameCopyId and GameId exist, and if they do, it removes that GameCopy from the database. If the GameCopy does not exist, it throws a KeyNotFoundException.
    public async Task<Unit> Handle(RemoveGameCopyCommand request, CancellationToken cancellationToken)
    {
        var gameCopy = await _db.GameCopies.FirstOrDefaultAsync(gc => gc.Id == request.GameCopyId && gc.GameId == request.GameId, cancellationToken); // Checks if the GameCopy with the provided GameCopyId and GameId exists in the database
        if (gameCopy == null) throw new KeyNotFoundException($"GameCopy with ID {request.GameCopyId} for Game ID {request.GameId} was not found.");

        _db.GameCopies.Remove(gameCopy); // Removes the GameCopy from the database context
        await _db.SaveChangesAsync(cancellationToken); // Saves the changes to the database, which will delete the GameCopy record

        return Unit.Value; // Returns Unit to indicate that the command has been handled successfully
    }
}