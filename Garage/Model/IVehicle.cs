using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Model
{
    public interface IVehicle
    {
        string LicenseNumber { get; }
        string Colour { get; }
        int NrOfWheels { get; }
    }
}
