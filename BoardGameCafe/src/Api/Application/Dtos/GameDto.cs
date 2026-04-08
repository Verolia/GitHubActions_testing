using Api.Domain.Enums;

namespace Api.Application.Dtos;

public record GameDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int MaxPlayers { get; init; }
    public int PlaytimeMin { get; init; }
    public GameComplexity Complexity { get; init; }
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}