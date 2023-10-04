using Garage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Controller
{
    internal interface IHandler
    {
        //Lista samtliga parkerade fordon
        IEnumerable<IVehicle> GetParkedVehicles();
        //Lista fordonstyper och hur många av varje som står i garaget
        string GetVehicleTypesCount();
        //Lägga till och ta bort fordon ur garaget
        bool AddVehicle(IVehicle vehicle);
        bool RemoveVehicle(string licenseNr);
        //Möjlighet att populera garaget med ett antal fordon från start
        bool AddVehicles(IVehicle[] vehicles);
        //Hitta ett specifikt fordon via registreringsnumret. Det ska gå fungera med både ABC123 samt Abc123 eller AbC123
        IVehicle GetVehicleByLicense(string licenseNumber);

        /* Användaren ska få feedback på att saker gått bra eller dåligt.
         * Till exempel när vi parkerat ett fordon vill vi få en bekräftelse på att fordonet är parkerat. 
         * Om det inte går vill användaren få veta varför.
        */

        //Från gränssnittet skall det gå att:
        //Navigera till samtlig funktionalitet från garage via gränssnittet
        //Skapa ett garage med en användar specificerad storlek

        //Sätta en kapacitet(antal parkeringsplatser) vid instansieringen av ett nytt garage
        bool CreateGarage(int capacity);
        
        bool LicenseIsUnique(string license);
        int CheckForFreeSpots();

        //Applikationen skall fel hantera indata på ett robust sätt, så att den inte kraschar vid felaktig inmatning eller användning.
    }
}
