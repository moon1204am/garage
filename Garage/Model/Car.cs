namespace Garage.Model
{
    public class Car : Vehicle
    {
        public double CylinderVolume { get; }

        public Car(string licenseNumber, string colur, int nrOfWheels, double cylinderVolume) : base(licenseNumber, colur, nrOfWheels)
        {
            CylinderVolume = cylinderVolume;
        }

        public override string ToString() => $"{base.ToString()}\nCylinder volume: {CylinderVolume}";

    }
}
