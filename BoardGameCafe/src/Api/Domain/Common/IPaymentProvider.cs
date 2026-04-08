namespace Api.Domain.Common;

public interface IPaymentProvider{
    Task<bool> ProcessPayment(decimal amount);
}