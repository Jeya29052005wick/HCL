using OnlineShoppingCartSystem.Models;
using OnlineShoppingCartSystem.Services;
using OnlineShoppingCartSystem.Payments;
using OnlineShoppingCartSystem.Utilities;

namespace OnlineShoppingCartSystem;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("==============================================");
        Console.WriteLine("   ONLINE SHOPPING CART & ORDER MANAGEMENT");
        Console.WriteLine("==============================================");

        string name = InputHelper.ReadNonEmpty("Enter customer name: ");
        string email = InputHelper.ReadNonEmpty("Enter customer email: ");
        var customer = new Customer(1, name, email);

        var products = new Product[]
        {
            new(101, "Keyboard", 899.00m, 15, "Accessories"),
            new(102, "Mouse", 499.00m, 20, "Accessories"),
            new(103, "Monitor", 8999.00m, 8, "Electronics"),
            new(104, "USB-C Cable", 249.00m, 30, "Accessories"),
            new(105, "Headphones", 1499.00m, 10, "Audio"),
            new(106, "Laptop Stand", 1199.00m, 12, "Accessories")
        };

        var cart = new ShoppingCart();
        var service = new ShoppingService(products);
        Order? latestOrder = null;

        while (true)
        {
            Console.WriteLine("\n--------------- MAIN MENU ---------------");
            Console.WriteLine("1. Browse products");
            Console.WriteLine("2. Search products");
            Console.WriteLine("3. Sort products by name");
            Console.WriteLine("4. Add product to cart");
            Console.WriteLine("5. Remove product from cart");
            Console.WriteLine("6. View cart and totals");
            Console.WriteLine("7. Checkout / place order");
            Console.WriteLine("8. View latest order summary");
            Console.WriteLine("9. Demonstrate C# concepts (record, tuple, casting)");
            Console.WriteLine("0. Exit");

            int option = InputHelper.ReadInt("Choose an option: ", 0, 9);
            switch (option)
            {
                case 1:
                    service.ListProducts();
                    break;
                case 2:
                    SearchProducts(service);
                    break;
                case 3:
                    service.SortProductsByName();
                    service.ListProducts();
                    break;
                case 4:
                    AddToCart(service, cart);
                    break;
                case 5:
                    RemoveFromCart(cart);
                    break;
                case 6:
                    ShowCart(cart);
                    break;
                case 7:
                    latestOrder = Checkout(service, customer, cart, latestOrder);
                    break;
                case 8:
                    if (latestOrder is null)
                        Console.WriteLine("No order has been placed in this session yet.");
                    else
                        latestOrder.PrintSummary();
                    break;
                case 9:
                    ShowConceptDemo(products[0]);
                    break;
                case 0:
                    Console.WriteLine("Thank you for shopping. Goodbye!");
                    return;
                default:
                    // The input validator prevents this branch, but it is useful as a defensive default.
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }

    private static void SearchProducts(ShoppingService service)
    {
        string search = InputHelper.ReadNonEmpty("Enter product name (or part of a name): ");
        var matches = service.SearchProducts(search);
        if (matches.Count == 0)
        {
            Console.WriteLine("No matching products were found.");
            return;
        }

        Console.WriteLine("\nSearch results:");
        foreach (Product product in matches)
            PrintProduct(product);
    }

    private static void AddToCart(ShoppingService service, ShoppingCart cart)
    {
        service.ListProducts();
        int id = InputHelper.ReadInt("Enter product ID: ", 1, int.MaxValue);
        Product? product = service.FindProductById(id);
        if (product is null)
        {
            Console.WriteLine("Product ID not found.");
            return;
        }

        if (!InputHelper.TryReadQuantity("Enter quantity: ", out CartQuantity quantity))
        {
            Console.WriteLine("Invalid quantity. Enter a whole number greater than zero.");
            return;
        }

        // Overload 1 accepts CartQuantity, a small value-type wrapper around an integer.
        if (cart.AddItem(product, quantity, out string message))
            Console.WriteLine(message);
        else
            Console.WriteLine($"Could not add item: {message}");
    }

    private static void RemoveFromCart(ShoppingCart cart)
    {
        if (cart.IsEmpty)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        cart.PrintItems();
        int id = InputHelper.ReadInt("Enter product ID to remove: ", 1, int.MaxValue);
        int quantity = InputHelper.ReadInt("Quantity to remove (use 0 to remove all): ", 0, int.MaxValue);

        // The overloaded RemoveItem method treats quantity 0 as remove-all.
        bool removed = quantity == 0
            ? cart.RemoveItem(id)
            : cart.RemoveItem(id, quantity);

        Console.WriteLine(removed ? "Cart updated." : "That product is not in the cart, or the quantity is invalid.");
    }

    private static void ShowCart(ShoppingCart cart)
    {
        if (cart.IsEmpty)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        cart.PrintItems();
        var totals = cart.CalculateTotals();
        var summary = new CartSummary(totals.Subtotal, totals.Discount, 0m);
        PrintCartSummary(in summary);
        Console.WriteLine("Shipping is calculated during checkout after you choose a delivery method.");
        Console.WriteLine($"Subtotal: {totals.Subtotal:C}");
        Console.WriteLine($"Automatic discount: {totals.Discount:C}");
        Console.WriteLine($"Subtotal after discount: {totals.Subtotal - totals.Discount:C}");
    }

    private static Order? Checkout(
        ShoppingService service,
        Customer customer,
        ShoppingCart cart,
        Order? latestOrder)
    {
        if (cart.IsEmpty)
        {
            Console.WriteLine("Cannot checkout: your cart is empty.");
            return latestOrder;
        }

        cart.PrintItems();
        var totals = cart.CalculateTotals();
        Console.WriteLine($"\nSubtotal: {totals.Subtotal:C}");
        Console.WriteLine($"Automatic discount: {totals.Discount:C}");

        // Coupon entry is optional; blank means no coupon.
        Console.Write("Coupon code (press Enter to skip; SAVE10 gives 10%, capped at 500): ");
        string coupon = Console.ReadLine() ?? "";
        decimal couponDiscount = service.CalculateCouponDiscount(totals.Subtotal - totals.Discount, coupon, out string couponMessage);
        if (!string.IsNullOrWhiteSpace(coupon))
            Console.WriteLine(couponMessage);

        decimal discount = Math.Min(totals.Subtotal, totals.Discount + couponDiscount);
        Console.WriteLine($"Total discount: {discount:C}");

        Console.WriteLine("\nShipping method:");
        Console.WriteLine("1. Store pickup (free)");
        Console.WriteLine("2. Standard delivery (100 currency units)");
        Console.WriteLine("3. Express delivery (250 currency units)");
        int shippingChoice = InputHelper.ReadInt("Choose shipping: ", 1, 3);

        string address = "Store pickup";
        AddressCoordinate coordinate = new(0, 0);
        if (shippingChoice != 1)
        {
            address = InputHelper.ReadNonEmpty("Delivery address: ");
            Console.Write("Optional latitude (press Enter to use 0): ");
            string latText = Console.ReadLine() ?? "";
            Console.Write("Optional longitude (press Enter to use 0): ");
            string lonText = Console.ReadLine() ?? "";
            double.TryParse(latText, out double latitude);
            double.TryParse(lonText, out double longitude);
            coordinate = new AddressCoordinate(latitude, longitude);
        }

        decimal shipping = shippingChoice switch
        {
            1 => 0m,
            2 => 100m,
            3 => 250m,
            _ => 0m
        };
        decimal payable = totals.Subtotal - discount + shipping;
        Console.WriteLine($"\nAmount payable: {payable:C}");

        IPaymentProcessor processor = ChoosePaymentProcessor();
        if (processor is null)
        {
            Console.WriteLine("No payment method selected.");
            return latestOrder;
        }

        Console.Write("Simulate payment success? (y/n): ");
        bool paymentApproved = (Console.ReadLine() ?? "").Trim().Equals("y", StringComparison.OrdinalIgnoreCase);

        // The processor is called through a common interface. This is a simulated payment only.
        if (!processor.ProcessPayment(payable, paymentApproved, out string paymentMessage))
        {
            Console.WriteLine($"Payment failed: {paymentMessage}");
            Console.WriteLine("Order was not placed and stock was not changed.");
            return latestOrder;
        }

        Order order = shippingChoice switch
        {
            1 => new StorePickupOrder(service.NextOrderId(), customer, cart, totals.Subtotal, discount, shipping, address),
            2 => new StandardDeliveryOrder(service.NextOrderId(), customer, cart, totals.Subtotal, discount, shipping, address, coordinate),
            3 => new ExpressDeliveryOrder(service.NextOrderId(), customer, cart, totals.Subtotal, discount, shipping, address, coordinate),
            _ => throw new InvalidOperationException("Unsupported shipping option.")
        };

        if (!service.TryPlaceOrder(order, cart, out string orderMessage))
        {
            Console.WriteLine(orderMessage);
            return latestOrder;
        }

        // ref passes the actual variable by reference, so the method updates the customer's points.
        int updatedPoints = customer.LoyaltyPoints;
        ShoppingService.AwardLoyaltyPoints(payable, ref updatedPoints);
        customer.LoyaltyPoints = updatedPoints;
        Console.WriteLine(orderMessage);
        Console.WriteLine($"Loyalty points: {customer.LoyaltyPoints}");
        order.PrintSummary();
        return order;
    }

    private static IPaymentProcessor ChoosePaymentProcessor()
    {
        Console.WriteLine("\nPayment method:");
        Console.WriteLine("1. Card (implicit interface implementation)");
        Console.WriteLine("2. UPI (explicit interface implementation)");
        int method = InputHelper.ReadInt("Choose payment method: ", 1, 2);

        IPaymentProcessor processor = method switch
        {
            1 => new CardPaymentProcessor(),
            2 => new UpiPaymentProcessor(),
            _ => new CardPaymentProcessor()
        };

        // Safe casting with 'as' and pattern matching with 'is'.
        IPaymentProcessor? safeReference = processor as IPaymentProcessor;
        if (safeReference is CardPaymentProcessor card)
            Console.WriteLine($"Selected: {card.MethodName}");
        else if (safeReference is UpiPaymentProcessor)
            Console.WriteLine("Selected: UPI");

        return processor;
    }

    private static void ShowConceptDemo(Product product)
    {
        ProductSummary first = product.ToSummary();
        ProductSummary second = new(product.Id, product.Name, product.Price, product.Category);
        Console.WriteLine($"Record equality (same data): {first == second}");

        ProductSummary saleSummary = first with { Price = first.Price * 0.90m };
        Console.WriteLine($"Original record price: {first.Price:C}");
        Console.WriteLine($"'with' copy with 10% off: {saleSummary.Price:C}");

        var cartTotals = (Subtotal: 1000m, Discount: 100m, Shipping: 100m);
        Console.WriteLine($"Named tuple final: {cartTotals.Subtotal - cartTotals.Discount + cartTotals.Shipping:C}");

        object boxedId = product.Id; // Boxing: value type -> object reference.
        int unboxedId = (int)boxedId; // Unboxing: object -> value type.
        Console.WriteLine($"Boxing/unboxing product ID: {unboxedId}");

        IPaymentProcessor payment = new CardPaymentProcessor();
        Console.WriteLine(payment is CardPaymentProcessor ? "Pattern matching found a card processor." : "Other processor.");
    }

    private static void PrintCartSummary(in CartSummary summary)
    {
        // 'in' passes a struct by readonly reference: it cannot be reassigned through this parameter.
        Console.WriteLine($"Read-only cart summary: subtotal {summary.Subtotal:C}, discount {summary.Discount:C}");
    }

    private static void PrintProduct(Product product)
    {
        Console.WriteLine($"{product.Id,4} | {product.Name,-18} | {product.Price,10:C} | Stock: {product.Stock,3} | {product.Category}");
    }
}
