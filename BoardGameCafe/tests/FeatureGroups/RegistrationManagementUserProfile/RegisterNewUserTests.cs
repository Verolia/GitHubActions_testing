/* using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Infrastructure.Behaviors;
using Api.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Tests;

public class RegisterUserCommandTests
{
    private IMediator _mediator = null!;
    private AccountManagementDbContext _db = null!;
    private ServiceProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // EF Core InMemory DB
        services.AddDbContext<AccountManagementDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddLogging();

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<RegisterUserCommand>();

        // Validation pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        _provider = services.BuildServiceProvider();

        _mediator = _provider.GetRequiredService<IMediator>();
        _db = _provider.GetRequiredService<AccountManagementDbContext>();

        SeedTestData(_db);
    }

    private void SeedTestData(AccountManagementDbContext db)
    {
        // Existing user to test duplicate email case
        db.Users.Add(new User(
            name: "Existing User",
            emailAddress: "existing@test.com",
            phone: "",
            passwordHash: "hashed",
            role: Api.Domain.Enums.UserRole.Customer
        ));

        db.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _db?.Dispose();
        _provider?.Dispose();
    }

    [Test]
    // Test that a new user can be registered successfully
    public async Task RegisterUserCommand_ShouldCreateUser()
    {
        var command = new RegisterUserCommand(
            Name: "John Doe",
            Email: "john@test.com",
            Password: "secret123"
        );

        var user = await _mediator.Send(command);

        Assert.NotNull(user);
        Assert.AreEqual("John Doe", user.Name);
        Assert.AreEqual("john@test.com", user.EmailAddress);

        // Verify password was "hashed" (reversed)
        Assert.AreEqual("321terces", user.PasswordHash);

        // Verify saved to DB
        var savedUser = await _db.Users.FirstOrDefaultAsync(u => u.EmailAddress == "john@test.com");
        Assert.NotNull(savedUser);

        TestContext.Progress.WriteLine(
            $"\nRegistered User with Id: {user.Id}, Name: {user.Name}, Email: {user.EmailAddress}");
    }

    [Test]
    // Test that registering with an existing email throws an exception
    public void RegisterUserCommand_NameRequired()
    {
        var command = new RegisterUserCommand(
            Name: "",
            Email: "test@test.com",
            Password: "secret123"
        );

        Assert.ThrowsAsync<ValidationException>(() =>
            _mediator.Send(command));
    }

    [Test]
    // Test that registering with an empty email throws an exception
    public void RegisterUserCommand_EmailRequired()
    {
        var command = new RegisterUserCommand(
            Name: "John Doe",
            Email: "",
            Password: "secret123"
        ); 
        Assert.ThrowsAsync<ValidationException>(() =>
            _mediator.Send(command));
    }

    [Test]
    // Test that registering with an invalid email format throws an exception
    public void RegisterUserCommand_EmailInvalid()
    {
        var command = new RegisterUserCommand(
            Name: "John Doe",
            Email: "invalid-email",
            Password: "secret123"
        );
        Assert.ThrowsAsync<ValidationException>(() =>
            _mediator.Send(command));
    }

    [Test]
    // Test that registering with a short password throws an exception
    public void RegisterUserCommand_PasswordTooShort()
    {
        var command = new RegisterUserCommand(
            Name: "John Doe",
            Email: "john@test.com",
            Password: "short"
        );
        Assert.ThrowsAsync<ValidationException>(() =>
            _mediator.Send(command));
    }

    [Test]
    // Test that empty username throws a validation error
    public void RegisterUserCommand_DuplicateEmail()
    {
        var command = new RegisterUserCommand(
            Name: "",
            Email: "existing@test.com",
            Password: "secret123"
        );
        Assert.ThrowsAsync<ValidationException>(() =>
            _mediator.Send(command));
    }
} */