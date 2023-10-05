using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.View
{
    internal class VehicleTypes
    {
        public const string Airplane = "1";
        public const string Boat = "2";
        public const string Bus = "3";
        public const string Car = "4";
        public const string Motorcycle = "5";
        public const string All = "6";

    }

    public enum VehicleType
    {
        Airplane = 1,
        Motorcycle = 2,
        Car = 3,
        Bus = 4,
        Boat = 5,
        All = 6
    }
}
