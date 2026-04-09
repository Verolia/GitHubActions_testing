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
using Api.Infrastructure.Persistence.GameCatalogManagement;

namespace Tests;

public class DeleteGameCommandTests
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
            cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly);
        });

        // Register validators
        services.AddValidatorsFromAssemblyContaining<CreateGameCommand>();

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
        var game = new Game
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Title = "Catan",
            MaxPlayers = 4,
            PlaytimeMin = 60,
            Complexity = Api.Domain.Enums.GameComplexity.Complex,
            Description = "A game of trading and building.",
            ImageUrl = "http://example.com/catan.jpg"
        };

        var gameCopy = new GameCopy
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000012"),
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
    public async Task DeleteGameCommand_Should_Delete_Game_Successfully()
    {
        TestContext.Progress.WriteLine("Starting DeleteGameCommand_Should_Delete_Game_Successfully test...");
        
        var gameIdToDelete = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var command = new DeleteGameCommand(gameIdToDelete);
        await _mediator.Send(command);
        var deletedGame = await _db.Games.FindAsync(gameIdToDelete);
        Assert.IsNull(deletedGame, "The game should have been deleted from the database.");
        var associatedGameCopies = await _db.GameCopies.Where(gc => gc.GameId == gameIdToDelete).ToListAsync();
        Assert.IsEmpty(associatedGameCopies, "All associated GameCopies should have been deleted from the database.");

        TestContext.Progress.WriteLine($"Deleted Game with Id: {gameIdToDelete} and all associated GameCopies successfully.");
    }

    [Test]
    public void DeleteGameCommand_Should_Throw_KeyNotFoundException_For_NonExistent_Game()
    {
        TestContext.Progress.WriteLine("Starting DeleteGameCommand_Should_Throw_KeyNotFoundException_For_NonExistent_Game test...");
        
        var nonExistentGameId = Guid.Parse("00000000-0000-0000-0000-000000000099");
        var command = new DeleteGameCommand(nonExistentGameId);
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _mediator.Send(command));
        Assert.That(ex!.Message, Is.EqualTo($"Game with ID {nonExistentGameId} not found."));

        TestContext.Progress.WriteLine($"Attempted to delete non-existent Game with Id: {nonExistentGameId} and received expected exception.");
    }

    [Test]
    public void DeleteGameCommand_Should_Throw_ValidationException_For_Invalid_GameId()
    {
        TestContext.Progress.WriteLine("Starting DeleteGameCommand_Should_Throw_ValidationException_For_Invalid_GameId test...");
        
        var invalidGameId = Guid.Empty;
        var command = new DeleteGameCommand(invalidGameId);
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));
        Assert.That(ex!.Errors, Has.Some.Matches<FluentValidation.Results.ValidationFailure>(f => f.ErrorMessage == "Invalid game ID."));

        TestContext.Progress.WriteLine($"Attempted to delete Game with invalid Id: {invalidGameId} and received expected validation exception.");
    }

}