namespace Garage.View
{
    /// <summary>
    /// Class responsible for the user interface. Reads inputs, displays user commands and prints information to the user.
    /// </summary>
    internal class ConsoleUI : IUI
    {
        public void Display() => Print("Welcome. " 
            + "\n1. Create garage." 
            + "\n2. Load a previous garage." 
            + "\n3. Load garage from configuration file.");
            
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

        public void DisplayFuelTypes()
        {
            Print("1. Diesel" 
                + "\n2. Gasoline");
        }

        public void DisplayVehicleOptions(bool isQuery = false)
        {

            foreach (VehicleType item in Enum.GetValues(typeof(VehicleType)))
            {
                if (!isQuery && item == VehicleType.All) continue;
                Print($"{(int)item} {item}");
            }
        }

        public string GetInput() => Console.ReadLine()!;


        public void Print(string msg) => Console.WriteLine(msg);

    }
}
