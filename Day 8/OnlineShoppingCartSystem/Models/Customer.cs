namespace OnlineShoppingCartSystem.Models;

public sealed class Customer
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public int LoyaltyPoints { get; set; }

    public Customer(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
        LoyaltyPoints = 0;
    }

    public override string ToString() => $"{Name} ({Email})";
}
