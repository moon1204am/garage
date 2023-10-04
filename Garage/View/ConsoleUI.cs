using Garage.Controller;
using Garage.Model;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Garage.View
{
    internal class ConsoleUI : IUI
    {

        public void Display()
        {
            Print("Welcome. Please create garage first.");
            
        }

        public void DisplayCommands()
        {
            Print("\n1. List all parked vehicles"
                    + "\n2. List how many of each vehicle types"
                    + "\n3. Park a vehicle"
                    + "\n4. Check out vehicle"
                    + "\n5  Create new garage"
                    + "\n6  Populate garage with multiple vehicles"
                    + "\n7  Find vehicle by license number"
                    + "\n8  Custom query"
                    + "\n0. Shut down the application");
        }

        public void DisplayVehicleOptions(bool isQuery = false)
        {
            Print("1. Airplane" +
                "\n2. Boat" +
                "\n3. Bus" +
                "\n4. Car" +
                "\n5. Motorcycle");
            
            if(isQuery)
                Print("\n6. All");
        }

        public string GetInput()
        {
            return Console.ReadLine();
        }

        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
