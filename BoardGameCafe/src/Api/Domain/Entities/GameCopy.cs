using Api.Domain.Enums;
using Api.Domain.Common;

namespace Api.Domain.Entities;

// GameCopy defines a specific instance of some Game as it is added by the Admin.
public class GameCopy : Entity, IAggregateRoot
{
    public Guid GameId {get; set;} // Reference to another aggregate
    private GameCopyStatus _status;
    public GameCopyStatus Status => _status;
    private GameCopyCondition _condition;
    public GameCopyCondition Condition => _condition;
    
    public bool IsAvalable => Status == GameCopyStatus.Available;

    // Navigation property
    public Game Game { get; set; } = null!;
    // This associates this Domain entity to the Game Domain entity in this example.

    
    // TODO: implement state-change limitation, eg. Only Available games can be reserved
    public void SetReserved(){ this._status = GameCopyStatus.Reserved; }
    public void SetAvailable(){ _status = GameCopyStatus.Available; }
    public void SetCheckedOut(){ _status = GameCopyStatus.CheckedOut; }
    public void SetMissing(){ _status = GameCopyStatus.Missing; }
    public void MarkConditionGood(){ _condition = GameCopyCondition.Good; }
    public void MarkConditionOkay(){ _condition = GameCopyCondition.Okay; }
    public void MarkAsDamaged(){ _condition = GameCopyCondition.Damaged; }
}

    