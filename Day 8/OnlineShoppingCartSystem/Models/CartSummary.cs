namespace OnlineShoppingCartSystem.Models;

// Readonly value type used with the 'in' parameter modifier.
public readonly struct CartSummary
{
    public decimal Subtotal { get; }
    public decimal Discount { get; }
    public decimal Shipping { get; }

    public CartSummary(decimal subtotal, decimal discount, decimal shipping)
    {
        Subtotal = subtotal;
        Discount = discount;
        Shipping = shipping;
    }
}
