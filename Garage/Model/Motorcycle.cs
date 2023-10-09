namespace Garage.Model
{
    /// <summary>
    /// Class representing a Motorcycle.
    /// </summary>
    internal class Motorcycle : Vehicle
    {
        public int NrOfSeats { get; set; }
        public Motorcycle(string licenseNumber, string colour, int nrOfWheels, int nrOfSeats) : base(licenseNumber, colour, nrOfWheels)
        {
            NrOfSeats = nrOfSeats;
        }

        public override string ToString() => $"{base.ToString()}\nNr of seats: {NrOfSeats}";
    }
}
