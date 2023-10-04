using Garage.Controller;
using Garage.Model;
using Garage.View;

namespace Garage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHandler handler = new GarageHandler();
            Manager manager = new Manager(handler);
            manager.Start();

            //var v = new Vehicle("", ", 4");
            //var res = v.GetType().GetProperties();

            //Activator.CreateInstance()
        }
    }
}