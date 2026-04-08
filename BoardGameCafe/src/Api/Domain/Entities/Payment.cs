using Api.Domain.Common;
using Api.Domain.Enums;

namespace Api.Domain.Entities;

public class Payment : Entity, IAggregateRoot
{
    public Guid ReservationId {get;} // Ref to another aggregate
    private PaymentType Type {get;}
    public IPaymentProvider? Provider {get;}
    public int AmountNOK {get;}
    public string Currency {get;} = "";
    private PaymentStatus _status;
    public PaymentStatus Status;
    public string ProviderRef {get;} = "";
    
    public void SetPending(){_status = PaymentStatus.Pending;}
    public void SetFailed(){_status = PaymentStatus.Failed;}
    public void SetCaptured(){_status = PaymentStatus.Captured;} // Captured is a finance term, separate into PaymentContext later?
}