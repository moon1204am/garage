using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Model
{
    internal class Boat : Vehicle
    {
        public double Length { get; }
        public Boat(string licenseNumber, string colur, int nrOfWheels, double length) : base(licenseNumber, colur, nrOfWheels)
        {
            Length = length;
        }
        public override string ToString() => $"{base.ToString()}\nLength: {Length}";

    }
}
