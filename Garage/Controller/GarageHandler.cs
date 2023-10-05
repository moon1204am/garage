using Garage.Model;
using Garage.View;

namespace Garage.Controller
{
    /* En GarageHandler. För att abstrahera ett lager så att det inte finns någon direkt
     * kontakt mellan användargränssnittet och garage klassen. Detta görs lämpligen
     * genom en klass som hanterar funktionaliteten som gränssnittet behöver ha tillgång till.
    */
    public class GarageHandler : IHandler
    {

        Garage<IVehicle> garage = default!;
        public bool AddVehicle(IVehicle vehicle)
        {
            return garage.Insert(vehicle);
        }

        public bool AddVehicles(IVehicle[] vehicles)
        {
            bool added = true;
            for (int i = 0; i < vehicles.Length; i++) 
            {
                added = AddVehicle(vehicles[i]);
                if (!added) added = false;
            }
            return added;
        }

        public bool CreateGarage(int capacity)
        {
            if (capacity == 0) return false;
            garage = new Garage<IVehicle>(capacity);
            return true;
        }

        public int CheckForFreeSpots() => garage!.IsFull ? 0 : garage.GetFreeSpaces;


        public bool RemoveVehicle(string licenseNr)
        {
            var vehicleToRemove = GetVehicleByLicense(licenseNr);
            if(vehicleToRemove == null) return false;
            return garage!.Remove(vehicleToRemove);
        }

        public bool LicenseAlreadyExists(string license) => garage!.Any(v => v.LicenseNumber == license);

        public IVehicle GetVehicleByLicense(string licenseNumber) => garage.FirstOrDefault(v => v.LicenseNumber == licenseNumber);
     

        public IEnumerable<VehicleDTO> GetCountOfEachType()
        {
            var counts = garage!.GroupBy(vehicle => vehicle.GetType().Name).Select(g => new VehicleDTO(g.Key, g.Count()));
            return counts;
        }

        public IEnumerable<IVehicle> GetParkedVehicles() => garage!.Select(v => v);

        public IEnumerable<IVehicle> CustomQuery(IVehicle vehicle)
        {
            IEnumerable<IVehicle> test = garage!.Where(v => v.LicenseNumber == vehicle.LicenseNumber || v.Colour == vehicle.Colour || v.NrOfWheels == vehicle.NrOfWheels);
            return test;
        }

        private bool SomeFunc(Vehicle v)
        {
            return v.LicenseNumber == "X";
        }

        public IEnumerable<IVehicle> CustomQuery(IVehicle vehicle, VehicleType vehicleType)
        {
            IEnumerable<IVehicle> result = VehicleType.All == vehicleType ? garage : garage.Where(v => v.GetType().Name == Enum.GetName<VehicleType>(vehicleType));
            result = result.Where(v => v.LicenseNumber == (vehicle.LicenseNumber == "X" ? v.LicenseNumber : vehicle.LicenseNumber));
            result = result.Where(v => v.Colour == (vehicle.Colour == "X" ? v.Colour : vehicle.Colour));
            result = result.Where(v => v.NrOfWheels == (vehicle.NrOfWheels == 0 ? v.NrOfWheels : vehicle.NrOfWheels));

            return result;
        }
    }
}
