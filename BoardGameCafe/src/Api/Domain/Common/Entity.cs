namespace Api.Domain.Common;

// Every domain entity has an ID and a creation-time it is what makes them entities and not value-objects
public abstract class Entity
{
    public Guid Id { get; set; }
    public int CreatedAt { get; set; } = 0;
}