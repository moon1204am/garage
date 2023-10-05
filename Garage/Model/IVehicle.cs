namespace Garage.Model
{
    public interface IVehicle
    {
        string LicenseNumber { get; set; }
        string Colour { get; set; }
        int NrOfWheels { get; }
    }
}
