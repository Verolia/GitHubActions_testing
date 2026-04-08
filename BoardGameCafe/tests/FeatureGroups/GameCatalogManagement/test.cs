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
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Add DbContext with InMemory provider
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        // Add MediatR
        services.AddMediatR(typeof(CreateGameCommand).Assembly);

        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateGameCommandValidator>();

        // Add pipeline behavior for validation
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    public async Task CreateGameCommand_Should_Create_Game()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var command = new CreateGameCommand
        {
            Name = "Catan",
            Description = "A popular board game",
            MinPlayers = 3,
            MaxPlayers = 4,
            PlayTimeMinutes = 60,
            AgeRating = AgeRating.TenPlus
        };

        // Act
        var result = await mediator.Send(command);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.Description, result.Description);
        Assert.AreEqual(command.MinPlayers, result.MinPlayers);
        Assert.AreEqual(command.MaxPlayers, result.MaxPlayers);
        Assert.AreEqual(command.PlayTimeMinutes, result.PlayTimeMinutes);
        Assert.AreEqual(command.AgeRating, result.AgeRating);
    }
}