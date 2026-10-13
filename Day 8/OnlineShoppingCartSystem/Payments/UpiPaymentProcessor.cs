namespace OnlineShoppingCartSystem.Payments;

public sealed class UpiPaymentProcessor : IPaymentProcessor
{
    // Explicit interface implementation: access these members through IPaymentProcessor.
    string IPaymentProcessor.MethodName => "UPI";

    bool IPaymentProcessor.ProcessPayment(decimal amount, bool simulateSuccess, out string message)
    {
        if (simulateSuccess)
        {
            message = $"Simulated UPI payment of {amount:C} approved.";
            return true;
        }

        message = "The simulated UPI transaction failed or was cancelled.";
        return false;
    }
}
