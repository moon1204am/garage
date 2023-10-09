namespace Garage.Model
{
    /// <summary>
    /// Data transfer object of vehicle only including type of vehicle and how many of each vehicle.
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="NrOfType"></param>
    public record VehicleDTO(string Type, int NrOfType);
}
