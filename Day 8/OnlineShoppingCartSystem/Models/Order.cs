namespace OnlineShoppingCartSystem.Models;

public abstract class Order
{
    public int OrderId { get; }
    public Customer Customer { get; }
    public IReadOnlyList<CartItem> Items { get; }
    public decimal Subtotal { get; }
    public decimal Discount { get; }
    public decimal Shipping { get; }
    public decimal Total => Subtotal - Discount + Shipping;
    public string Address { get; }

    protected Order(int orderId, Customer customer, ShoppingCart cart,
        decimal subtotal, decimal discount, decimal shipping, string address)
    {
        OrderId = orderId;
        Customer = customer;
        Items = cart.CreateSnapshot();
        Subtotal = subtotal;
        Discount = discount;
        Shipping = shipping;
        Address = address;
    }

    public virtual string FulfillmentDescription => "General order";

    public virtual void PrintSummary()
    {
        Console.WriteLine("\n================ ORDER SUMMARY ================");
        Console.WriteLine($"Order ID: {OrderId}");
        Console.WriteLine($"Customer: {Customer}");
        Console.WriteLine($"Fulfillment: {FulfillmentDescription}");
        Console.WriteLine($"Address / pickup: {Address}");
        foreach (CartItem item in Items)
            Console.WriteLine($"  {item.Quantity} x {item.Product.Name} = {item.LineTotal:C}");
        Console.WriteLine($"Subtotal: {Subtotal:C}");
        Console.WriteLine($"Discount: -{Discount:C}");
        Console.WriteLine($"Shipping: {Shipping:C}");
        Console.WriteLine($"FINAL TOTAL: {Total:C}");
        Console.WriteLine("================================================");
    }
}

public sealed class StorePickupOrder : Order
{
    public StorePickupOrder(int orderId, Customer customer, ShoppingCart cart,
        decimal subtotal, decimal discount, decimal shipping, string address)
        : base(orderId, customer, cart, subtotal, discount, shipping, address) { }

    public override string FulfillmentDescription => "Store pickup (free)";
    public override void PrintSummary()
    {
        Console.WriteLine("Pickup order created.");
        base.PrintSummary();
    }
}

public abstract class DeliveryOrder : Order
{
    public AddressCoordinate Coordinate { get; }

    protected DeliveryOrder(int orderId, Customer customer, ShoppingCart cart,
        decimal subtotal, decimal discount, decimal shipping, string address, AddressCoordinate coordinate)
        : base(orderId, customer, cart, subtotal, discount, shipping, address)
    {
        Coordinate = coordinate;
    }

    public override void PrintSummary()
    {
        base.PrintSummary();
        Console.WriteLine($"Delivery coordinates: {Coordinate}");
    }
}

public sealed class StandardDeliveryOrder : DeliveryOrder
{
    public StandardDeliveryOrder(int orderId, Customer customer, ShoppingCart cart,
        decimal subtotal, decimal discount, decimal shipping, string address, AddressCoordinate coordinate)
        : base(orderId, customer, cart, subtotal, discount, shipping, address, coordinate) { }

    public override string FulfillmentDescription => "Standard delivery";
}

public sealed class ExpressDeliveryOrder : DeliveryOrder
{
    public ExpressDeliveryOrder(int orderId, Customer customer, ShoppingCart cart,
        decimal subtotal, decimal discount, decimal shipping, string address, AddressCoordinate coordinate)
        : base(orderId, customer, cart, subtotal, discount, shipping, address, coordinate) { }

    public override string FulfillmentDescription => "Express delivery";
}
