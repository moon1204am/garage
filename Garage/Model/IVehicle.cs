namespace Garage.Model
{
    /// <summary>
    /// Interface representing a vehicle.
    /// </summary>
    public interface IVehicle
    {
        string LicenseNumber { get; }
        string Colour { get; }
        int NrOfWheels { get; }
    }
}
