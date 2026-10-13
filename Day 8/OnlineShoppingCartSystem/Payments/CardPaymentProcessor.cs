namespace OnlineShoppingCartSystem.Payments;

public sealed class CardPaymentProcessor : IPaymentProcessor
{
    public string MethodName => "Card";

    // Implicit interface implementation: public member matches the interface member.
    public bool ProcessPayment(decimal amount, bool simulateSuccess, out string message)
    {
        if (simulateSuccess)
        {
            message = $"Simulated card payment of {amount:C} approved.";
            return true;
        }

        message = "The simulated card issuer declined the payment.";
        return false;
    }
}
