using OnlineShoppingCartSystem.Models;

namespace OnlineShoppingCartSystem.Utilities;

public static class InputHelper
{
    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = (Console.ReadLine() ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(value)) return value;
            Console.WriteLine("This field cannot be empty.");
        }
    }

    public static int ReadInt(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out int value) && value >= minimum && value <= maximum)
                return value;

            Console.WriteLine($"Enter a whole number from {minimum} to {maximum}.");
        }
    }

    // 'out' returns both success/failure and the parsed CartQuantity value.
    public static bool TryReadQuantity(string prompt, out CartQuantity quantity)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        if (int.TryParse(input, out int value) && value > 0)
        {
            quantity = new CartQuantity(value);
            return true;
        }

        quantity = default;
        return false;
    }
}
