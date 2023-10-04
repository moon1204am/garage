using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Model
{
    internal class VehicleDTO
    {
        public string Type { get; set; }
        public int NrOfType { get; set; }
    }

    internal record VehicleDTO2(string Type, int NrOfType);
}
