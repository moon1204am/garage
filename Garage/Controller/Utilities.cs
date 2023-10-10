using Garage.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Garage.Controller
{
    /// <summary>
    /// Class that is responsible for saving, updating and loading garages.
    /// </summary>
    public class Utilities
    {
        IConfiguration config;

        public Utilities(IConfiguration config)
        {
            this.config = config;
        }

        public bool IsLoaded { get; set; }

        public bool Save(string name, int capacity, IEnumerable<IVehicle> parkedVehicles)
        {
            string path = $"{Environment.CurrentDirectory}/{name}.txt";
            if (File.Exists(path))
                throw new Exception("Name already taken.");

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"Capacity:{capacity}");
                WriteVehicles(sw, parkedVehicles);
                
            }
            return true;
        }

        internal bool Update(string name, IEnumerable<IVehicle> newVehicles)
        {
            string path = $"{Environment.CurrentDirectory}/{name}.txt";
            if (!File.Exists(path))
                throw new Exception("Name did not exist.");

            using (StreamWriter sw = File.AppendText(path))
            {
                WriteVehicles(sw, newVehicles);
            }
            return true;
        }

        private void WriteVehicles(StreamWriter sw, IEnumerable<IVehicle> vehicles)
        {
            var options = new JsonSerializerOptions();
            foreach (var v in vehicles)
            {
                var jsonVehicle = JsonSerializer.Serialize(v, v.GetType(), options);
                sw.WriteLine($"Type:{GetVehicleType(v)}");
                sw.WriteLine(jsonVehicle);
            }
        }

        public bool Load(string name, Func<int, bool> create, Func<IVehicle, bool> add)
        {
            string path = $"{Environment.CurrentDirectory}/{name}.txt";
            if(!File.Exists(path))
                throw new Exception("Name did not exist.");
                

            using (StreamReader reader = new StreamReader(path))
            {
                string capacityString = reader.ReadLine()!;
                string[] split = capacityString.Split(":");
                int capacity = int.TryParse(split[1], out capacity) ? capacity : 0;
                if (capacity < 1) return false;
                create(capacity);
                string type = string.Empty;

                while (!reader.EndOfStream)
                {
                    var json = reader.ReadLine()!;
                    if (json.Contains("Type:"))
                    {
                        split = json.Split(":");
                        type = split[1];
                        json = reader.ReadLine()!;
                    }
                    if (!string.IsNullOrEmpty(type))
                    {
                        IVehicle? vehicleObj = null;
                        switch (type)
                        {
                            case "2":
                                vehicleObj = JsonSerializer.Deserialize<Airplane>(json);
                                break;
                            case "3":
                                vehicleObj = JsonSerializer.Deserialize<Boat>(json);
                                break;
                            case "4":
                                vehicleObj = JsonSerializer.Deserialize<Bus>(json);
                                break;
                            case "5":
                                vehicleObj = JsonSerializer.Deserialize<Car>(json);
                                break;
                            case "6":
                                vehicleObj = JsonSerializer.Deserialize<Motorcycle>(json);
                                break;
                        }
                        add(vehicleObj!);
                    }
                }
                IsLoaded = true;
                return true;
            }
        }

        public string GetVehicleType(IVehicle v)
        {
            if (v is Airplane)
                return "2";
            else if (v is Boat)
                return "3";
            else if (v is Bus)
                return "4";
            else if (v is Car)
                return "5";
            else if (v is Motorcycle)
                return "6";
            return "0";
        }

        internal int ReadCapacityFromConfig()
        {
            var capacity = config.GetSection(GarageSettings.pathCapacity).Value;
            return int.TryParse(capacity, out int res) ? res : 0;
        }
    }
}
