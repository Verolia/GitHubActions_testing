using Api.Domain.Common;
using Api.Domain.Enums;

namespace Api.Domain.Entities;

public class WaitlistEntry : Entity, IAggregateRoot
{
    private WaitlistEntryStatus _status;
    public WaitlistEntryStatus Status => _status;
    
    // TODO: check state-change validity for strict invariants/business logic
    public void SetWaiting(){_status = WaitlistEntryStatus.Waiting;}
    public void SetNotified(){_status = WaitlistEntryStatus.Notified;}
    public void SetCancelled(){_status = WaitlistEntryStatus.Cancelled;}
}