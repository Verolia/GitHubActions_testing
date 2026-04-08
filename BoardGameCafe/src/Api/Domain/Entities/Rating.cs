using Api.Domain.Common;

namespace Api.Domain.Entities;

public class Rating : Entity , IAggregateRoot
{
    public Guid CustomerId {get; set;}
    public Guid GameId {get; set;}
    public int Stars {get; set;}
    public string Comment {get; set;} = "";
}