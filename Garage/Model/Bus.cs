namespace Garage.Model
{
    internal class Bus : Vehicle
    {
        public string FuelType { get; }
        public Bus(string licenseNumber, string colur, int nrOfWheels, string fuelType) : base(licenseNumber, colur, nrOfWheels)
        {
            FuelType = fuelType;
        }
        public override string ToString() => $"{base.ToString()}\nFuel type: {FuelType}";
    }
}
