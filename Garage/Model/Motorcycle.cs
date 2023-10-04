using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Model
{
    internal class Motorcycle : Vehicle
    {
        public int NrOfSeats { get; }
        public Motorcycle(string licenseNumber, string colur, int nrOfWheels, int nrOfSeats) : base(licenseNumber, colur, nrOfWheels)
        {
            NrOfSeats = nrOfSeats;
        }

        public override string ToString() => $"{base.ToString()}\nNr of seats: {NrOfSeats}";
    }
}
