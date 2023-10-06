namespace Garage.View
{
    internal class ConsoleUI : IUI
    {

        public void Display() => Print("Welcome. " +
            "\n1. Please create garage first. " +
            "\n2. Or load a previous garage.");
            
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

            foreach (VehicleType item in Enum.GetValues(typeof(VehicleType)))
            {
                if (!isQuery && item == VehicleType.All) continue;
                Print($"{(int)item} {item}");
            }
        }

        public string GetInput() => Console.ReadLine();


        public void Print(string msg) => Console.WriteLine(msg);

    }
}
