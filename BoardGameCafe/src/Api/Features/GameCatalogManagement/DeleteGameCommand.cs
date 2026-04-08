using MediatR;
using FluentValidation;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence.GameCatalogManagement;
using Api.Application.Dtos;

namespace Api.Features.GameCatalogManagement;

/*
Description
Takes in the Id of an existing Game and deletes it from the database. 
Also ensures that all associated GameCopies are deleted as well. 
Tests include successful deletion, attempting to delete a non-existent Game, and validation for invalid GameId.
*/

public record DeleteGameCommand(Guid Id) : IRequest<Unit>;

public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, Unit>
{
    private readonly GameCatalogManagementDbContext _db;

    public DeleteGameCommandHandler(GameCatalogManagementDbContext db)
    {
        _db = db;
    }

    public class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
    {
        public DeleteGameCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage("Invalid game ID.");
        }
    }

    public async Task<Unit> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _db.Games.FindAsync(new object[] { request.Id }, cancellationToken);
        var gameCopies = await _db.GameCopies.Where(gc => gc.GameId == request.Id).ToListAsync(cancellationToken);

        if (game == null)
        {
            throw new KeyNotFoundException($"Game with ID {request.Id} not found.");
        }

        _db.GameCopies.RemoveRange(gameCopies); // Removes all associated GameCopies from the database context

        _db.Games.Remove(game); // Removes the Game from the database context
        await _db.SaveChangesAsync(cancellationToken); // Saves the changes to the database

        return Unit.Value; // Returns Unit to indicate that the command has been handled successfully
    }
}