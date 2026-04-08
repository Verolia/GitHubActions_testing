using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using Api.Infrastructure.Persistence;
using Api.Infrastructure.Behaviors;
using Api.Features.Users;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Tests;

public class DeleteUserCommandTests
{
    private IMediator _mediator = null!;
    private AccountManagementDbContext _db = null!;
    private ServiceProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // EF Core InMemory database
        services.AddDbContext<AccountManagementDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        // Logging
        services.AddLogging();

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DeleteUserCommand).Assembly));

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<DeleteUserCommand>();

        // Validation pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        _provider = services.BuildServiceProvider();

        _mediator = _provider.GetRequiredService<IMediator>();
        _db = _provider.GetRequiredService<AccountManagementDbContext>();

        SeedTestData(_db);
    }

    private Guid _existingUserId;

    private void SeedTestData(AccountManagementDbContext db)
    {
        var user = new User(
            name: "Delete Me",
            emailAddress: "delete@test.com",
            phone: "",
            passwordHash: "hash",
            role: UserRole.Customer
        );

        db.Users.Add(user);
        db.SaveChanges();

        _existingUserId = user.Id;
    }

    [TearDown]
    public void TearDown()
    {
        _db?.Dispose();
        _provider?.Dispose();
    }


    [Test]
    // Test that a user can be successfully deleted
    public async Task DeleteUserCommand_ShouldDeleteUser()
    {
        var result = await _mediator.Send(new DeleteUserCommand(_existingUserId));

        Assert.IsTrue(result, "DeleteUserCommand should return true on success");

        var deletedUser = await _db.Users.FindAsync(_existingUserId);
        Assert.IsNull(deletedUser, "User should be removed from database");
    }

    [Test]
    // Test that attempting to delete a non-existing user throws KeyNotFoundException
    public void DeleteUserCommand_UserNotFound_ShouldThrow()
    {
        var nonExistingUserId = Guid.NewGuid();

        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _mediator.Send(new DeleteUserCommand(nonExistingUserId)));
    }


    [Test]
    // Test that attempting to delete user with an empty UserId throws a validation exception
    public void DeleteUserCommand_UserIdRequired_ShouldThrowValidationException()
    {
        Assert.ThrowsAsync<ValidationException>(() =>
            _mediator.Send(new DeleteUserCommand(Guid.Empty)));
    }
}

// TODO: Add test for authorization, can only delete if user is admin or deleting their own account, otherwise should throw an exception (e.g. UnauthorizedAccessException)