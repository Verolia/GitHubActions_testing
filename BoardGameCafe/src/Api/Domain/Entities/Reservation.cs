using System.Data;
using Api.Domain.Common;
using Api.Domain.Enums;

namespace Api.Domain.Entities;


// Reservation is an entity and an aggregate root for an aggregate that consist of only itself
// It implements IAggregateRoot to make this clear, as well as preparing it for publishing events (potentially).
public class Reservation : Entity , IAggregateRoot 
{
    public Guid CustomerId;
    public Guid Table {get;}
    public DateTime StartTime;
    public DateTime EndTime;
    public int PartySize;
    public List<Guid> ReservedGameCopies {get;} = [];
    private ReservationStatus _status = ReservationStatus.Submitted;
    public ReservationStatus Status => _status;

    //TODO ascertain valid State transitions, eg. a Cancelled reservation cannot be Seated.
    public void SetConfirmed(){ _status = ReservationStatus.Confirmed; } 
    public void SetSeated(){ _status = ReservationStatus.Seated; } 
    public void SetCompleted(){ _status = ReservationStatus.Completed; } 
    public void SetCancelled(){ _status = ReservationStatus.Cancelled; } 
    public void SetNoShow(){ _status = ReservationStatus.NoShow; } 
}
