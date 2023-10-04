using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Model
{
    public class Airplane : Vehicle
    {
        public int NrOfEngines { get; }
        public Airplane(string licenseNumber, string colour, int nrOfWheels, int nrOfEngines) : base(licenseNumber, colour, nrOfWheels)
        {
            NrOfEngines = nrOfEngines;
        }

        public override string ToString() => $"{base.ToString()}\nNr of engines: {NrOfEngines }";

    }
}
