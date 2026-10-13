namespace OnlineShoppingCartSystem.Models;

// A struct is a value type. This one keeps quantity validation in one place.
public readonly struct CartQuantity
{
    public int Value { get; }

    public CartQuantity(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be greater than zero.");
        Value = value;
    }

    public override string ToString() => Value.ToString();
}
