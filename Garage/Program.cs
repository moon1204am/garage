using Garage.Controller;
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
        }
    }
}