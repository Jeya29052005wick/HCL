using OnlineShoppingCartSystem.Services;
namespace OnlineShoppingCartSystem.Models;

public sealed class ShoppingCart
{
    private readonly List<CartItem> _items = new();
    public IReadOnlyList<CartItem> Items => _items;
    public bool IsEmpty => _items.Count == 0;

    // Overload 1: quantity as an int.
    public bool AddItem(Product product, int quantity, out string message)
    {
        if (quantity <= 0)
        {
            message = "Quantity must be greater than zero.";
            return false;
        }

        CartItem? existing = _items.FirstOrDefault(item => item.Product.Id == product.Id);
        int alreadyInCart = existing?.Quantity ?? 0;
        if (product.Stock < alreadyInCart + quantity)
        {
            message = $"Only {product.Stock - alreadyInCart} more item(s) can be added.";
            return false;
        }

        if (existing is null)
            _items.Add(new CartItem(product, quantity));
        else
            existing.IncreaseQuantity(quantity);

        message = $"{quantity} x {product.Name} added to cart.";
        return true;
    }

    // Overload 2: quantity wrapped in the CartQuantity struct.
    public bool AddItem(Product product, CartQuantity quantity, out string message) =>
        AddItem(product, quantity.Value, out message);

    // Overload 1: remove a specific quantity.
    public bool RemoveItem(int productId, int quantity)
    {
        CartItem? item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item is null || quantity <= 0 || quantity > item.Quantity) return false;

        if (quantity == item.Quantity)
            _items.Remove(item);
        else
            item.DecreaseQuantity(quantity);
        return true;
    }

    // Overload 2: remove the entire product line.
    public bool RemoveItem(int productId)
    {
        CartItem? item = _items.FirstOrDefault(i => i.Product.Id == productId);
        return item is not null && _items.Remove(item);
    }

    public (decimal Subtotal, decimal Discount, decimal Shipping) CalculateTotals(decimal shipping = 0m)
    {
        decimal subtotal = _items.Sum(item => item.LineTotal);
        decimal discount = DiscountCalculator.CalculateDiscount(subtotal);
        return (Subtotal: subtotal, Discount: discount, Shipping: shipping);
    }

    public void PrintItems()
    {
        Console.WriteLine("\n---------------- YOUR CART ----------------");
        Console.WriteLine($"{"ID",4} | {"Product",-18} | {"Qty",3} | {"Unit price",12} | {"Line total",12}");
        foreach (CartItem item in _items)
            Console.WriteLine($"{item.Product.Id,4} | {item.Product.Name,-18} | {item.Quantity,3} | {item.Product.Price,12:C} | {item.LineTotal,12:C}");
    }

    public List<CartItem> CreateSnapshot() => _items.Select(item => item.Copy()).ToList();

    public void Clear() => _items.Clear();
}
