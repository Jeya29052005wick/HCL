namespace OnlineShoppingCartSystem.Services;

public static class DiscountCalculator
{
    // Overload 1: automatic store discount, 5% when subtotal is at least 5,000.
    public static decimal CalculateDiscount(decimal subtotal)
    {
        if (subtotal >= 5000m)
            return Math.Round(subtotal * 0.05m, 2);
        return 0m;
    }

    // Overload 2: coupon discount. SAVE10 is 10%, capped at 500.
    public static decimal CalculateDiscount(decimal eligibleAmount, string couponCode, out string message)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
        {
            message = "No coupon applied.";
            return 0m;
        }

        if (couponCode.Trim().Equals("SAVE10", StringComparison.OrdinalIgnoreCase))
        {
            decimal discount = Math.Min(eligibleAmount * 0.10m, 500m);
            message = $"Coupon SAVE10 accepted. Coupon discount: {discount:C}.";
            return Math.Round(discount, 2);
        }

        message = "Invalid coupon code. No coupon discount applied.";
        return 0m;
    }
}
