using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Api.Infrastructure.Persistence.GameCatalogManagement;
using Api.Features.GameCopies;
using Api.Domain.Entities;
using FluentValidation;
using Api.Infrastructure.Behaviors;

namespace Tests;

public class AddGameCopyCommandTests
{
    private IMediator _mediator = null!;
    private GameCatalogManagementDbContext _db = null!;
    private ServiceProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Use EF Core InMemory database
        services.AddDbContext<GameCatalogManagementDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        // Adds logging
        services.AddLogging(); 

        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AddGameCopyCommand).Assembly);
        });

        // Register validators
        services.AddValidatorsFromAssemblyContaining<AddGameCopyCommand>();

        // Add FluentValidation pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        _provider = services.BuildServiceProvider();
        

        _mediator = _provider.GetRequiredService<IMediator>();
        _db = _provider.GetRequiredService<GameCatalogManagementDbContext>();

        // Seed required data directly into the InMemory DB
        SeedTestData(_db);
    }

    private void SeedTestData(GameCatalogManagementDbContext db)
    {
        db.Games.Add(new Game
        {
            Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef"),
            Title = "Catan",
            MaxPlayers = 4,
            PlaytimeMin = 60,
            Complexity = Api.Domain.Enums.GameComplexity.Complex,
            Description = "A game of trading and building."
        });

        db.SaveChanges();
    }


    [TearDown]
    public void TearDown()
    {
        _db?.Dispose();
        _provider?.Dispose();
    }


    [Test]
    public async Task AddGameCopyCommand_ShouldAddGameCopy()
    // Attempt to add a GameCopy for an existing Game
    {
        TestContext.Progress.WriteLine("\nStarting AddGameCopyCommand_ShouldAddGameCopy test...");
        var gameId = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef");

        var gameCopy = await _mediator.Send(new AddGameCopyCommand(gameId));

        Assert.NotNull(gameCopy);
        Assert.AreEqual(gameId, gameCopy.GameId);

        TestContext.Progress.WriteLine(
            $"Added GameCopy with Id: {gameCopy.Id} for GameId: {gameCopy.GameId}");
    }

    [Test]
    public async Task AddGameCopyCommand_GameCopyIdRequiredError()
    // Attempt to add a GameCopy with null GameCopyId
    {
        TestContext.Progress.WriteLine("\nStarting AddGameCopyCommand_GameCopyIdRequiredError test...");
        var ex = Assert.ThrowsAsync<ValidationException>(async () =>
            await _mediator.Send(new AddGameCopyCommand(Guid.Empty)));

        TestContext.Progress.WriteLine(
            $"Attempted to add GameCopy with empty GameId, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }

    [Test]
    public async Task AddGameCopyCommand_GameIdNotFoundError()
    // Attempt to add a GameCopy for a non-existent GameId
    {
        TestContext.Progress.WriteLine("\nStarting AddGameCopyCommand_GameIdNotFoundError test...");
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _mediator.Send(new AddGameCopyCommand(Guid.Parse("d1bd1bfa-4d48-4d69-a68d-0fd1599095ea"))));

        TestContext.Progress.WriteLine(
            $"Attempted to add GameCopy for non-existent GameId, caught exception: {ex.GetType().Name} with message: {ex.Message}");

    }
}

