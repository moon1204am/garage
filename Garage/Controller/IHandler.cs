using Garage.Model;
using Garage.View;

namespace Garage.Controller
{
    public interface IHandler
    {
        int GetCount { get; }
        bool IsLoaded { get; }
        IEnumerable<IVehicle> GetParkedVehicles();
        IEnumerable<VehicleDTO> GetCountOfEachType();
        bool AddVehicle(IVehicle vehicle);
        bool RemoveVehicle(string licenseNr);
        IVehicle? GetVehicleByLicense(string licenseNumber);
        bool CreateGarage(int capacity);
        bool LicenseAlreadyExists(string license);
        int CheckForFreeSpots();
        IEnumerable<IVehicle> CustomQuery(IVehicle vehicle, VehicleType type);
        bool Save(string name);
        bool Load(string input);
        bool Update(string name);
        bool CreateGarageFromConfig();
    }
}
