namespace OnlineShoppingCartSystem.Payments;

public interface IPaymentProcessor
{
    string MethodName { get; }
    bool ProcessPayment(decimal amount, bool simulateSuccess, out string message);
}
