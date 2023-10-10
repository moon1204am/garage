namespace Garage.Model
{
    /// <summary>
    /// Class representing a vehicle.
    /// </summary>
    public class Vehicle : IVehicle
    {
        public string LicenseNumber { get; }
        public string Colour { get; }
        public int NrOfWheels { get; }

        public Vehicle(string licenseNumber, string colur, int nrOfWheels) 
        { 
            LicenseNumber = licenseNumber;
            Colour = colur;
            NrOfWheels = nrOfWheels;
        }

        public override string ToString()
        {
            return $"Vehicle type: {this.GetType().Name}\nLicense number: {LicenseNumber}\nColour: {Colour}\nNr of wheels: {NrOfWheels}";
        }
    }
}
