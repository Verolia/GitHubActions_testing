using Api.Domain.Common;
using Api.Domain.Enums;

namespace Api.Domain.Entities;

// Game defines a boardgame that is available for playing at the café
public class Game : Entity, IAggregateRoot
{
    public string Title {get; set;} = "";
    public int MaxPlayers {get; set;} = 0;
    public int PlaytimeMin {get; set;} = 0;
    public GameComplexity Complexity {get; set;} = GameComplexity.Simple;
    public String Description {get; set;} = "";
    public string ImageUrl {get; set;} = "";

    // Navigation property for related GameCopy entities
    public ICollection<GameCopy> GameCopies { get; set; }  = new List<GameCopy>();
}