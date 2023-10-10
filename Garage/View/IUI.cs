namespace Garage.View
{
    internal interface IUI
    {
        string GetInput();
        void Print(string msg);
        void Display();
        void DisplayCommands();
        void DisplayVehicleOptions(bool isQuery = false);
        void DisplayFuelTypes();
    }
}
