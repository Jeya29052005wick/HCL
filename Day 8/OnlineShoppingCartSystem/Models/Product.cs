namespace OnlineShoppingCartSystem.Models;

public sealed class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Stock { get; private set; }
    public string Category { get; }

    public Product(int id, string name, decimal price, int stock, string category)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
        if (stock < 0) throw new ArgumentOutOfRangeException(nameof(stock));

        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        Category = category;
    }

    public bool HasStock(int quantity) => quantity > 0 && Stock >= quantity;

    public bool TryReduceStock(int quantity)
    {
        if (!HasStock(quantity)) return false;
        Stock -= quantity;
        return true;
    }

    public void RestoreStock(int quantity)
    {
        if (quantity > 0) Stock += quantity;
    }

    public ProductSummary ToSummary() => new(Id, Name, Price, Category);
}

// Records compare their values, not just their object references.
public record ProductSummary(int Id, string Name, decimal Price, string Category);
