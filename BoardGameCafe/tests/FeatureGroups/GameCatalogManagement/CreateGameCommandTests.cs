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

public class CreateGameCommandTests
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
    }


    [TearDown]
    public void TearDown()
    {
        _db?.Dispose();
        _provider?.Dispose();
    }


    [Test]
    public async Task CreateGameCommand_ShouldCreateGame()
    {
        // Arrange
        TestContext.Progress.WriteLine("\nStarting CreateGameCommand_ShouldCreateGame test...");
        var gameDto = new GameCreateDto
        {
            Title = "Test Game",
            MaxPlayers = 4,
            PlaytimeMin = 30,
            Complexity = GameComplexity.Simple,
            Description = "A test game for unit testing.",
            ImageUrl = "http://example.com/image.jpg"
        };
        var command = new CreateGameCommand(gameDto);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(gameDto.Title, result.Title);
        Assert.AreEqual(gameDto.MaxPlayers, result.MaxPlayers);
        Assert.AreEqual(gameDto.PlaytimeMin, result.PlaytimeMin);
        Assert.AreEqual(gameDto.Complexity, result.Complexity);
        Assert.AreEqual(gameDto.Description, result.Description);
        Assert.AreEqual(gameDto.ImageUrl, result.ImageUrl);

        TestContext.Progress.WriteLine(
            $"Created Game with Id: {result.Id}, Title: {result.Title}");
    }

    [Test]
    public async Task CreateGameCommand_MissingTitle_ShouldFailValidation()
    {
        TestContext.Progress.WriteLine("\nStarting CreateGameCommand_MissingTitle_ShouldFailValidation test...");
        var gameDto = new GameCreateDto
        {
            Title = "", // Missing title
            MaxPlayers = 4,
            PlaytimeMin = 30,
            Complexity = GameComplexity.Simple,
            Description = "A test game with missing title.",
            ImageUrl = "http://example.com/image.jpg"
        };
        var command = new CreateGameCommand(gameDto);

        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));

        TestContext.Progress.WriteLine(
            $"Attempted to create Game with missing title, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }

    [Test]
    public async Task CreateGameCommand_InvalidMaxPlayers_ShouldFailValidation()
    {
        TestContext.Progress.WriteLine("\nStarting CreateGameCommand_InvalidMaxPlayers_ShouldFailValidation test...");
        var gameDto = new GameCreateDto
        {
            Title = "Test Game",
            MaxPlayers = 0, // Invalid MaxPlayers
            PlaytimeMin = 30,
            Complexity = GameComplexity.Simple,
            Description = "A test game with invalid MaxPlayers.",
            ImageUrl = "http://example.com/image.jpg"
        };
        var command = new CreateGameCommand(gameDto);

        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));

        TestContext.Progress.WriteLine(
            $"Attempted to create Game with invalid MaxPlayers, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }

    [Test]
    public async Task CreateGameCommand_InvalidPlaytimeMin_ShouldFailValidation()   
    {
        TestContext.Progress.WriteLine("\nStarting CreateGameCommand_InvalidPlaytimeMin_ShouldFailValidation test...");
        var gameDto = new GameCreateDto
        {
            Title = "Test Game",
            MaxPlayers = 4,
            PlaytimeMin = 0, // Invalid PlaytimeMin
            Complexity = GameComplexity.Simple,
            Description = "A test game with invalid PlaytimeMin.",
            ImageUrl = "http://example.com/image.jpg"
        };
        var command = new CreateGameCommand(gameDto);

        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command));

        TestContext.Progress.WriteLine(
            $"Attempted to create Game with invalid PlaytimeMin, caught exception: {ex.GetType().Name} with message: {ex.Message}");
    }
}

