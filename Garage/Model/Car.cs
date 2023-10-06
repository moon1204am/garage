namespace Garage.Model
{
    public class Car : Vehicle
    {
        public double CylinderVolume { get; }

        public Car(string licenseNumber, string colour, int nrOfWheels, double cylinderVolume) : base(licenseNumber, colour, nrOfWheels)
        {
            CylinderVolume = cylinderVolume;
        }

        public override string ToString() => $"{base.ToString()}\nCylinder volume: {CylinderVolume}";

    }
}
