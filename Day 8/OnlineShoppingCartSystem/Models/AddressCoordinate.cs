namespace OnlineShoppingCartSystem.Models;

// A second value-type struct for optional delivery coordinates.
public readonly struct AddressCoordinate
{
    public double Latitude { get; }
    public double Longitude { get; }

    public AddressCoordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public override string ToString() => $"({Latitude}, {Longitude})";
}
