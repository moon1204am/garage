namespace Garage.Model
{
    /// <summary>
    /// Data transfer object of vehicle only including type of vehicle and how many of each vehicle.
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="NrOfType"></param>
    public record VehicleDTO(string Type, int NrOfType);

    //internal class VehicleDTO2
    //{
    //    public string LicenseNumber { get; set; }
    //    public string Colour { get; set; }
    //    public string NrOfWheels { get; set; }
    //}
}
