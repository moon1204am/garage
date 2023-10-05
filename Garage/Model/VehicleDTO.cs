namespace Garage.Model
{
    public record VehicleDTO(string Type, int NrOfType);

    internal class VehicleDTO2
    {
        public string LicenseNumber { get; set; }
        public string Colour { get; set; }
        public string NrOfWheels { get; set; }
    }
}
