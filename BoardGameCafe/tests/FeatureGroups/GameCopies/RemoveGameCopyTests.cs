using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Api.Infrastructure.Persistence;
using Api.Features.GameCopies;
using Api.Domain.Entities;
using FluentValidation;
using Api.Infrastructure.Behaviors;

  

namespace Tests;

public class RemoveGameCopyCommandTests
{
    private IMediator _mediator = null!;
    private MainDbContext _db = null!;
    private ServiceProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Use EF Core InMemory database
        services.AddDbContext<MainDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        // Adds logging
        services.AddLogging(); 

        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RemoveGameCopyCommand).Assembly);
        });
        
        // Register validators
        services.AddValidatorsFromAssemblyContaining<RemoveGameCopyCommand>();

        // Add FluentValidation pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        _provider = services.BuildServiceProvider();
        

        _mediator = _provider.GetRequiredService<IMediator>();
        _db = _provider.GetRequiredService<MainDbContext>();

        // Seed required data directly into the InMemory DB
        SeedTestData(_db);
    }

    private void SeedTestData(MainDbContext db)
    {
        var game = new Game
        {
            Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"),
            Title = "Catan",
            MaxPlayers = 4,
            PlaytimeMin = 60,
            Complexity = Api.Domain.Enums.GameComplexity.Complex,
            Description = "A game of trading and building."
        };

        var gameCopy = new GameCopy
        {
            Id = Guid.Parse("a2bd1bfa-4d38-4d69-a68d-0fd1599095ef"),
            GameId = game.Id,
        };
        gameCopy.SetAvailable();
        gameCopy.MarkConditionGood();

        db.Games.Add(game);
        db.GameCopies.Add(gameCopy);

        db.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _db?.Dispose();
        _provider?.Dispose();
    }

    [Test]
    public async Task RemoveGameCopyCommand_ShouldRemoveGameCopy()
    // Attempt to remove an existing GameCopy
    {
        TestContext.Progress.WriteLine("\nStarting RemoveGameCopyCommand_ShouldRemoveGameCopy test...");
        var gameCopyId = Guid.Parse("a2bd1bfa-4d38-4d69-a68d-0fd1599095ef");
        var gameId = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea");

        await _mediator.Send(new RemoveGameCopyCommand(gameId, gameCopyId));

        var removedGameCopy = await _db.GameCopies.FindAsync(gameCopyId);
        Assert.IsNull(removedGameCopy);
        TestContext.Progress.WriteLine(
            $"Removed GameCopy with Id: {gameCopyId}");
        TestContext.Progress.WriteLine(
            $"Removed GameCopy is Null: {removedGameCopy == null}");
    }

    [Test]
    public async Task RemoveGameCopyCommand_GameCopyIdRequiredError()
    // Attempt to remove a GameCopy with null GameCopyId
    {
        TestContext.Progress.WriteLine("\nStarting RemoveGameCopyCommand_GameCopyIdRequiredError test...");
        var ex = Assert.ThrowsAsync<ValidationException>(async () =>
            await _mediator.Send(new RemoveGameCopyCommand(Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"), Guid.Empty)));

        TestContext.Progress.WriteLine(
            $"Attempted to remove GameCopy with empty GameCopyId, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }

    [Test]
    public async Task RemoveGameCopyCommand_GameIdRequiredError()
    {
        TestContext.Progress.WriteLine("\nStarting RemoveGameCopyCommand_GameIdRequiredError test...");
        var ex = Assert.ThrowsAsync<ValidationException>(async () =>
            await _mediator.Send(new RemoveGameCopyCommand(Guid.Empty, Guid.Parse("a2bd1bfa-4d38-4d69-a68d-0fd1599095ef"))));

        TestContext.Progress.WriteLine(
            $"Attempted to remove GameCopy with empty GameId, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }


    [Test]
    public async Task RemoveGameCopyCommand_GameCopyIdNotFoundError()
    // Attempt to remove a GameCopy with a non-existent GameCopyId
    {
        TestContext.Progress.WriteLine("\nStarting RemoveGameCopyCommand_GameCopyIdNotFoundError test...");
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _mediator.Send(new RemoveGameCopyCommand(Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"), Guid.Parse("b2bd1bfa-4d38-4d69-a68d-0fd1599095ef"))));

        TestContext.Progress.WriteLine(
            $"Attempted to remove GameCopy with non-existent GameCopyId, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }

}