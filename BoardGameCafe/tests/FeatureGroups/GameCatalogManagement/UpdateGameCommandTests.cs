using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using Api.Infrastructure.Persistence;
using Api.Domain.Entities;
using Api.Infrastructure.Behaviors;
using Api.Features.GameCatalogManagement;
using Api.Application.Dtos;
using Api.Domain.Enums;
using NUnit.Framework.Internal;

namespace Tests;

public class UpdateGameCommandTests
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
            cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly);
        });

        // Register validators
        services.AddValidatorsFromAssemblyContaining<CreateGameCommand>();

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
            Description = "A game of trading and building.",
            ImageUrl = "http://example.com/catan.jpg"
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
    public async Task UpdateGameCommand_ShouldUpdateGame()
    {
        // Arrange
        TestContext.Progress.WriteLine("\nStarting UpdateGameCommand_ShouldUpdateGame test...");
        var gameDto = new GameUpdateDto
        {
            Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"), // Existing game ID from seeded data
            Title = "Catan Updated",
            MaxPlayers = 5,
            PlaytimeMin = 90,
            Complexity = GameComplexity.Complex,
            Description = "An updated description for Catan.",
            ImageUrl = "http://example.com/Updatedcatan.jpg"
        };

        var command = new UpdateGameCommand(gameDto);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        var updatedGame = await _db.Games.FindAsync(gameDto.Id);
        Assert.IsNotNull(updatedGame);
        Assert.AreEqual(gameDto.Title, updatedGame!.Title);
        Assert.AreEqual(gameDto.MaxPlayers, updatedGame.MaxPlayers);
        Assert.AreEqual(gameDto.PlaytimeMin, updatedGame.PlaytimeMin);
        Assert.AreEqual(gameDto.Complexity, updatedGame.Complexity);
        Assert.AreEqual(gameDto.Description, updatedGame.Description);
        Assert.AreEqual(gameDto.ImageUrl, updatedGame.ImageUrl);



        // Verify that the changes are persisted in the database
        var updatedGameInDb = await _db.Games.FindAsync(gameDto.Id);
        Assert.IsNotNull(updatedGameInDb);
        Assert.AreEqual(gameDto.Title, updatedGameInDb!.Title);

        TestContext.Progress.WriteLine(
            $"Updated Game with Id: {gameDto.Id}, Title: {updatedGameInDb.Title}, MaxPlayers: {updatedGameInDb.MaxPlayers}, PlaytimeMin: {updatedGameInDb.PlaytimeMin}, Complexity: {updatedGameInDb.Complexity}, Description: {updatedGameInDb.Description}, ImageUrl: {updatedGameInDb.ImageUrl}");
    }

    [Test]
    public async Task UpdateGameCommand_MissingTitle_ShouldFailValidation()
    {
        TestContext.Progress.WriteLine("\nStarting UpdateGameCommand_MissingTitle_ShouldFailValidation test...");
        var gameDto = new GameUpdateDto
        {
            Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"), // Existing game ID from seeded data
            Title = "", // Missing title
            MaxPlayers = 5,
            PlaytimeMin = 90,
            Complexity = GameComplexity.Complex,
            Description = "An updated description for Catan.",
            ImageUrl = "http://example.com/catan.jpg"
        };

        var command = new UpdateGameCommand(gameDto);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));
        Assert.IsNotNull(ex);
        Assert.That(ex!.Errors, Has.Some.Matches<FluentValidation.Results.ValidationFailure>(f => f.PropertyName == "GameDTO.Title" && f.ErrorMessage == "Title is required."));

        TestContext.Progress.WriteLine("Validation failed as expected due to missing title.");
    }

    [Test]
    public async Task UpdateGameCommand_InvalidId_ShouldFailValidation()
    {
        TestContext.Progress.WriteLine("\nStarting UpdateGameCommand_InvalidId_ShouldFailValidation test...");
        var gameDto = new GameUpdateDto
        {
            Id = Guid.Empty, // Invalid ID
            Title = "Catan Updated",
            MaxPlayers = 5,
            PlaytimeMin = 90,
            Complexity = GameComplexity.Complex,
            Description = "An updated description for Catan.",
            ImageUrl = "http://example.com/catan.jpg"
        };

        var command = new UpdateGameCommand(gameDto);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));
        Assert.IsNotNull(ex);
        Assert.That(ex!.Errors, Has.Some.Matches<FluentValidation.Results.ValidationFailure>(f => f.PropertyName == "GameDTO.Id" && f.ErrorMessage == "Invalid game ID."));

        TestContext.Progress.WriteLine("Validation failed as expected due to invalid game ID.");

    }

    [Test]
    public async Task UpdateGameCommand_NonExistentId_ShouldThrowKeyNotFoundException()
    {
        TestContext.Progress.WriteLine("\nStarting UpdateGameCommand_NonExistentId_ShouldThrowKeyNotFoundException test...");
        var gameDto = new GameUpdateDto
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Non-existent ID
            Title = "Catan Updated",
            MaxPlayers = 5,
            PlaytimeMin = 90,
            Complexity = GameComplexity.Complex,
            Description = "An updated description for Catan.",
            ImageUrl = "http://example.com/catan.jpg"
        };

        var command = new UpdateGameCommand(gameDto);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _mediator.Send(command));
        Assert.IsNotNull(ex);
        Assert.That(ex!.Message, Is.EqualTo($"Game with ID {gameDto.Id} not found."));

        TestContext.Progress.WriteLine("KeyNotFoundException thrown as expected due to non-existent game ID.");
    }

    [Test]
    public async Task UpdateGameCommand_InvalidMaxPlayers_ShouldFailValidation()
    {
        TestContext.Progress.WriteLine("\nStarting UpdateGameCommand_InvalidMaxPlayers_ShouldFailValidation test...");
        var gameDto = new GameUpdateDto
        {
            Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"), // Existing game ID from seeded data
            Title = "Catan Updated",
            MaxPlayers = 0, // Invalid MaxPlayers
            PlaytimeMin = 90,
            Complexity = GameComplexity.Complex,
            Description = "An updated description for Catan.",
            ImageUrl = "http://example.com/catan.jpg"
        };

        var command = new UpdateGameCommand(gameDto);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));
        Assert.IsNotNull(ex);
        Assert.That(ex!.Errors, Has.Some.Matches<FluentValidation.Results.ValidationFailure>(f => f.PropertyName == "GameDTO.MaxPlayers" && f.ErrorMessage == "MaxPlayers must be greater than 0."));

        TestContext.Progress.WriteLine("Validation failed as expected due to invalid MaxPlayers.");
    }

    [Test]
    public async Task UpdateGameCommand_InvalidPlaytimeMin_ShouldFailValidation()
    {
        TestContext.Progress.WriteLine("\nStarting UpdateGameCommand_InvalidPlaytimeMin_ShouldFailValidation test...");
        var gameDto = new GameUpdateDto
        {
            Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1234095ea"), // Existing game ID from seeded data
            Title = "Catan Updated",
            MaxPlayers = 5,
            PlaytimeMin = 0, // Invalid PlaytimeMin
            Complexity = GameComplexity.Complex,
            Description = "An updated description for Catan.",
            ImageUrl = "http://example.com/catan.jpg"
        };

        var command = new UpdateGameCommand(gameDto);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));
        Assert.IsNotNull(ex);
        Assert.That(ex!.Errors, Has.Some.Matches<FluentValidation.Results.ValidationFailure>(f => f.PropertyName == "GameDTO.PlaytimeMin" && f.ErrorMessage == "PlaytimeMin must be greater than 0."));

        TestContext.Progress.WriteLine("Validation failed as expected due to invalid PlaytimeMin.");
    }

}