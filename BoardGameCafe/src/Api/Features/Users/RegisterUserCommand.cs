using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Persistence;

namespace Api.Features.Users;

public record RegisterUserCommand(string Name, string Email, string Password) : IRequest<User>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly MainDbContext _db;

    public RegisterUserCommandHandler(MainDbContext db)
    {
        _db = db;
    }

    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required.");

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");
        }
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var exists = await _db.Users.AnyAsync(u => u.EmailAddress == request.Email, cancellationToken);
        if (exists)
            throw new InvalidOperationException("Email already registered.");

        // Create user
        var user = new User(
            name: request.Name,
            emailAddress: request.Email,
            phone: "",
            passwordHash: HashPassword(request.Password),
            role: UserRole.Customer
        );

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return user;
    }

    // TODO - Simple password hashing placeholder, replace later
    private string HashPassword(string password)
    {
        // For now, just reverse the string as a dummy “hash”
        return new string(password.Reverse().ToArray());
    }
}

