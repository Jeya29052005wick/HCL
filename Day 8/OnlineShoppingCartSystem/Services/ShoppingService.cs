using OnlineShoppingCartSystem.Models;

namespace OnlineShoppingCartSystem.Services;

public sealed class ShoppingService
{
    private readonly Product[] _products;
    private int _nextOrderId = 1000;

    public ShoppingService(Product[] products) => _products = products;

    public void ListProducts()
    {
        Console.WriteLine("\n---------------- PRODUCT CATALOGUE ----------------");
        Console.WriteLine($"{"ID",4} | {"Name",-18} | {"Price",10} | {"Stock",5} | Category");
        for (int i = 0; i < _products.Length; i++)
        {
            Product product = _products[i];
            Console.WriteLine($"{product.Id,4} | {product.Name,-18} | {product.Price,10:C} | {product.Stock,5} | {product.Category}");
        }
    }

    public List<Product> SearchProducts(string name)
    {
        // Case-insensitive matching; partial matches are supported too.
        var matches = new List<Product>();
        foreach (Product product in _products)
        {
            if (!product.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                continue; // Demonstrates continue: skip products that do not match.

            matches.Add(product);
        }
        return matches;
    }

    public Product? FindProductById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public void SortProductsByName()
    {
        Array.Sort(_products, (first, second) =>
            string.Compare(first.Name, second.Name, StringComparison.OrdinalIgnoreCase));
    }

    public decimal CalculateCouponDiscount(decimal eligibleAmount, string coupon, out string message) =>
        DiscountCalculator.CalculateDiscount(eligibleAmount, coupon, out message);

    public bool TryPlaceOrder(Order order, ShoppingCart cart, out string message)
    {
        if (cart.IsEmpty)
        {
            message = "Cannot place an order with an empty cart.";
            return false;
        }

        // Validate all lines before changing any stock, avoiding partial stock updates.
        foreach (CartItem item in cart.Items)
        {
            if (!item.Product.HasStock(item.Quantity))
            {
                message = $"Insufficient stock for {item.Product.Name}. Order was not placed.";
                return false;
            }
        }

        var reducedItems = new List<CartItem>();
        foreach (CartItem item in cart.Items)
        {
            if (!item.Product.TryReduceStock(item.Quantity))
            {
                // Roll back any earlier reductions if a stock update unexpectedly fails.
                foreach (CartItem reduced in reducedItems)
                    reduced.Product.RestoreStock(reduced.Quantity);

                message = "Stock changed during checkout. Please try again.";
                return false;
            }
            reducedItems.Add(item);
        }

        cart.Clear();
        message = $"Order #{order.OrderId} placed successfully.";
        return true;
    }

    public int NextOrderId() => ++_nextOrderId;

    public static void AwardLoyaltyPoints(decimal amountPaid, ref int points)
    {
        // One loyalty point per complete 100 currency units paid.
        points += (int)(amountPaid / 100m);
    }
}
