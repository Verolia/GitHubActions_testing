using Api.Domain.Common;
using Api.Domain.Enums;

namespace Api.Domain.Entities;

// A Table defines the reserved seating area as reserved by the Customer
public class Table : Entity, IAggregateRoot
{
    public int Capacity {get;}
    private TableStatus Status {get; set;}
    
    public bool IsAvailable => Status == TableStatus.Available;
    public bool IsReserved => Status == TableStatus.Reserved;
}
