namespace OnlineShoppingCartSystem.Models;

public sealed class CartItem
{
    public Product Product { get; }
    public int Quantity { get; private set; }
    public decimal LineTotal => Product.Price * Quantity;

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount) => Quantity += amount;
    public bool DecreaseQuantity(int amount)
    {
        if (amount <= 0 || amount > Quantity) return false;
        Quantity -= amount;
        return true;
    }

    public CartItem Copy() => new(Product, Quantity);
}
