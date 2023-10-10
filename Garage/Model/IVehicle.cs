namespace Garage.Model
{
    public interface IVehicle
    {
        string LicenseNumber { get; }
        string Colour { get; }
        int NrOfWheels { get; }
    }
}
