# Online Shopping Cart and Order Management System

A beginner-friendly C#/.NET 8 console project. It uses in-memory sample products and simulated payments; it does **not** connect to a real payment gateway or database.

## Run the project

1. Install the .NET 8 SDK.
2. Extract `OnlineShoppingCartSystem.zip`.
3. Open a terminal in the `OnlineShoppingCartSystem` folder.
4. Run:

```bash
dotnet run
```

You can also open `OnlineShoppingCartSystem.csproj` in Visual Studio.

## Menu

- Browse, search, and sort products.
- Add and remove cart items.
- View line totals, subtotal, and automatic discount.
- Checkout with store pickup, standard delivery, or express delivery.
- Use simulated Card or UPI payment.
- View the latest order summary.
- Demonstrate record equality, the `with` expression, named tuples, boxing/unboxing, and pattern matching.

## Business rules

- Automatic discount: 5% when the subtotal is at least 5,000.
- Coupon: `SAVE10` applies an additional 10% coupon discount, capped at 500.
- Shipping: pickup = 0, standard = 100, express = 250.
- Loyalty: one point for every complete 100 currency units paid.
- Stock changes only after simulated payment succeeds and stock is revalidated.
- Currency formatting follows the computer's current culture.

## File structure

```text
OnlineShoppingCartSystem/
├── OnlineShoppingCartSystem.csproj
├── Program.cs
├── Models/
│   ├── Product.cs
│   ├── Customer.cs
│   ├── CartQuantity.cs
│   ├── AddressCoordinate.cs
│   ├── CartItem.cs
│   ├── CartSummary.cs
│   ├── ShoppingCart.cs
│   └── Order.cs
├── Services/
│   ├── DiscountCalculator.cs
│   └── ShoppingService.cs
├── Payments/
│   ├── IPaymentProcessor.cs
│   ├── CardPaymentProcessor.cs
│   └── UpiPaymentProcessor.cs
└── Utilities/
    └── InputHelper.cs
```

## Required concepts covered

| Concept | Where it appears |
|---|---|
| Value and reference types | `CartQuantity`, `AddressCoordinate`, `CartSummary` structs; classes such as `Product` and `Customer` |
| Operators, conversions, console I/O, interpolation | Throughout `Program.cs` and model/service classes |
| Conditions, switch statement and expression | Menu `switch`; shipping/payment switch expressions; validation `if` statements |
| `for`/`foreach`/`while` loops | `for` in `ShoppingService.ListProducts`, `foreach` over cart/search results, `while` for the menu and input validation |
| `break` and `continue` | `break` in menu `switch` cases; `continue` skips non-matching products during search |
| Validation | `InputHelper`, model constructors, cart stock checks, coupon validation |
| Overloaded cart/discount methods | `ShoppingCart.AddItem` / `RemoveItem`; `DiscountCalculator.CalculateDiscount` |
| `ref`, `out`, `in` | Loyalty points, quantity parsing, read-only `CartSummary` |
| Separate files/namespaces | `Models`, `Services`, `Payments`, `Utilities` |
| Abstract/base/virtual/override/polymorphism | `Order`, `DeliveryOrder`, `StorePickupOrder`, `StandardDeliveryOrder`, `ExpressDeliveryOrder` |
| Implicit and explicit interface implementations | Card and UPI payment processors |
| Interface reference, safe casting, pattern matching | `ChoosePaymentProcessor` and concept demo |
| Record equality and `with` | `ProductSummary` and concept demo |
| Named tuple | `ShoppingCart.CalculateTotals` |
| Coupon bonus | `SAVE10`, 10% discount with a 500 maximum |

## Suggested manual test plan

1. Search `mouse` and `MOUSE`: both should find Mouse.
2. Search `tablet`: no results should appear.
3. Add more than the displayed stock of any product: cart should reject it.
4. Enter `0`, letters, or a negative quantity: validation should reject it.
5. Add items worth less than 5,000: automatic discount should be zero.
6. Add items worth at least 5,000: automatic 5% discount should apply.
7. Compare pickup, standard, and express shipping: 0, 100, and 250.
8. Enter `SAVE10`: coupon should apply 10%, up to 500; invalid codes should not apply.
9. Choose payment failure (`n`): no order should be placed and stock should remain unchanged.
10. Choose payment success (`y`): order should be created, stock reduced, cart cleared, and loyalty points awarded.

## Notes for learning

- `decimal` is used for money to avoid binary floating-point rounding issues.
- The product catalogue is an array as required. Sorting changes the order of that array.
- This is an educational simulation. A production application would also need persistent storage, authenticated users, concurrency-safe inventory, payment gateway verification, and stronger address/payment validation.
