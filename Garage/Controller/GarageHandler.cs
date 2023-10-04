using Garage.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Controller
{
    /* En GarageHandler. För att abstrahera ett lager så att det inte finns någon direkt
     * kontakt mellan användargränssnittet och garage klassen. Detta görs lämpligen
     * genom en klass som hanterar funktionaliteten som gränssnittet behöver ha tillgång till.
    */
    public class GarageHandler : IHandler
    {

        Garage<IVehicle>? garage;
        public bool AddVehicle(IVehicle vehicle)
        {
            return garage!.Insert(vehicle);

           // foreach (var item in garage)
           // {
           //     Console.WriteLine(item);
           // }

           //var res =  garage.Where(v => v.Colour == "röd");

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

        public bool LicenseIsUnique(string license)
        {
            return !garage!.LicenseAlreadyExists(license);
        }

        public int CheckForFreeSpots()
        {
            return garage!.IsFull() ? 0 : garage.GetFreeSpaces();
        }

        public IEnumerable<IVehicle> GetParkedVehicles()
        {
            return garage!.GetAllVehicles();
        }

        public IVehicle GetVehicleByLicense(string licenseNumber)
        {
            return garage!.RetrieveVehicleByLicense(licenseNumber);
        }

        public string GetVehicleTypesCount()
        {
            return garage!.GetCountOfEachType();
        }

        public bool RemoveVehicle(string licenseNr)
        {
            return garage!.Remove(licenseNr);
        }
    }
}
