namespace Garage.Model
{
    internal class Boat : Vehicle
    {
        public double Length { get; }
        public Boat(string licenseNumber, string colour, int nrOfWheels, double length) : base(licenseNumber, colour, nrOfWheels)
        {
            Length = length;
        }
        public override string ToString() => $"{base.ToString()}\nLength: {Length}";

    }
}
