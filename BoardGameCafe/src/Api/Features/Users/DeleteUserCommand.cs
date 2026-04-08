using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence;

// This is a test for GitHub Actions. 

namespace Api.Features.Users;

public record DeleteUserCommand(Guid UserId) : IRequest<bool>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly MainDbContext _db;

    public DeleteUserCommandHandler(MainDbContext db)
    {
        _db = db;
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}