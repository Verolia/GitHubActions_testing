using Api.Domain.Enums;

namespace Api.Application.Dtos;

public record GameUpdateDto
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Title { get; init; } = "";
    public int MaxPlayers { get; init; } = 0;
    public int PlaytimeMin { get; init; } = 0;
    public GameComplexity Complexity { get; init; } = GameComplexity.Simple;
    public String Description { get; init; } = "";
    public string ImageUrl { get; init; } = "";
}