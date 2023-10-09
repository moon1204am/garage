using Garage.Controller;
using Garage.Model;
using Garage.View;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Garage
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Environment.CurrentDirectory)
                                        .AddJsonFile(GarageSettings.AppSettingsJson, optional: false, reloadOnChange: true)
                                        .Build();

            var host = Host.CreateDefaultBuilder(args)
                            .ConfigureServices(services => {
                                services.AddSingleton<IConfiguration>(config);
                                services.AddSingleton<Utilities>();
                                services.AddSingleton<IHandler, GarageHandler>();
                                services.AddSingleton<IUI, ConsoleUI>();
                                services.AddSingleton<Manager>();
                                
                            })
                            .UseConsoleLifetime()
                            .Build();

            host.Services.GetRequiredService<Manager>().Start();    

            //IHandler handler = new GarageHandler();
            //IUI ui = new ConsoleUI();
            //Manager manager = new Manager(handler, ui);
            //manager.Start();

            //IHandler handler = new GarageHandler();
            //handler.CreateGarage(3);
            //handler.AddVehicle(new Airplane("ABC123", "white", 3, 3));
            //handler.AddVehicle(new Motorcycle("abc124", "pink", 2, 2));
            //handler.AddVehicle(new Airplane("abc125", "pink", 3, 3));
            //var res = handler.CustomQuery(new Vehicle("ABC123", "X", 3), VehicleType.Airplane);
            //if(res.Any())
            //{
            //    foreach (var vehicle in res)
            //    {
            //        Console.WriteLine(vehicle);
            //    }
            //} else
            //{
            //    Console.WriteLine("Empty.");
            //}

            //var v = new Vehicle("", ", 4");
            //var res = v.GetType().GetProperties();

            //Activator.CreateInstance()
        }
    }
}