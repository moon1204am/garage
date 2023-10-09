using Garage.Model;
using Garage.View;

namespace Garage.Controller
{
    /// <summary>
    /// Class that acts as an abstraction layer between the user interface and the Garage class.
    /// </summary>
    public class GarageHandler : IHandler
    {
        Garage<IVehicle> garage = default!;
        Utilities util;

        public GarageHandler(Utilities util)
        {
            this.util = util;
        }

        //public int Capacity => garage.Capacity;

        public int GetCount => garage.GetCount;

        public bool IsLoaded => util.IsLoaded;

        public bool AddVehicle(IVehicle vehicle)
        {
            return garage.Insert(vehicle);
        }

        public bool CreateGarage(int capacity)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException();
            garage = new Garage<IVehicle>(capacity);
            return true;
        }

        public int CheckForFreeSpots() => garage.IsFull ? 0 : garage.GetFreeSpaces;

        public bool RemoveVehicle(string licenseNr)
        {
            var vehicleToRemove = GetVehicleByLicense(licenseNr);
            if(vehicleToRemove == null) return false;
            return garage.Remove(vehicleToRemove);
        }

        public bool LicenseAlreadyExists(string license) => garage.Any(v => v.LicenseNumber == license);

        public IVehicle GetVehicleByLicense(string licenseNumber) => garage.FirstOrDefault(v => v.LicenseNumber == licenseNumber);

        public IEnumerable<VehicleDTO> GetCountOfEachType()
        {
            var counts = garage.GroupBy(vehicle => vehicle.GetType().Name).Select(g => new VehicleDTO(g.Key, g.Count()));
            return counts;
        }

        public IEnumerable<IVehicle> GetParkedVehicles() => garage.Select(v => v);

        public IEnumerable<IVehicle> CustomQuery(IVehicle vehicle, VehicleType vehicleType)
        {
            IEnumerable<IVehicle> result = VehicleType.All == vehicleType ? garage : garage.Where(v => v.GetType().Name == Enum.GetName<VehicleType>(vehicleType));
            result = result.Where(v => v.LicenseNumber == (vehicle.LicenseNumber == "0" ? v.LicenseNumber : vehicle.LicenseNumber));
            result = result.Where(v => v.Colour == (vehicle.Colour == "0" ? v.Colour : vehicle.Colour));
            result = result.Where(v => v.NrOfWheels == (vehicle.NrOfWheels == 0 ? v.NrOfWheels : vehicle.NrOfWheels));

            return result;
        }

        public bool Save(string name)
        {
            if (garage.GetCount == 0)
                return false;

            return util.Save(name, garage.Capacity, GetParkedVehicles());
        }
        public bool Load(string input) => util.Load(input, CreateGarage, AddVehicle);

        //public bool GarageIsLoaded() =>  util.IsLoaded;

        public bool CreateGarageFromConfig()
        {
            int capacity = util.ReadCapacityFromConfig();
            return CreateGarage(capacity);
        }

        //public bool Update(string name)
        //{
        //    return util.Update(name);
        //}
    }
}
