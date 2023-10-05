namespace Garage.View
{
    internal interface IUI
    {
        string GetInput();
        void Print(string msg);
        //Det skall gå att stänga av applikationen från gränssnittet
        //void ShutDown();
        void Display();

    }
}
