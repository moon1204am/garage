using Garage.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
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

        string Name { get; set; } = null!;
        internal bool IsLoaded { get; set; }

        internal bool Save(string name, int capacity, IEnumerable<IVehicle> parkedVehicles)
        {
            if (Name == null) Name = name;
            else
                if (Name != name) return false;

            string path = $"{Environment.CurrentDirectory}/{name}.txt";
            if (File.Exists(path))
                throw new Exception("Name already taken.");

            WriteVehicles(parkedVehicles, name, capacity);
            return true;
        }

        internal bool Update(string name, IEnumerable<IVehicle> parkedVehicles, int capacity)
        {
            if (name != Name) return false;   
            string path = $"{Environment.CurrentDirectory}/{name}.txt";
            if (!File.Exists(path))
                throw new Exception("Name did not exist.");

            WriteVehicles(parkedVehicles, name, capacity);
            return true;
        }

        private void WriteVehicles(IEnumerable<IVehicle> vehicles, string path, int capacity)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"Capacity:{capacity}");
                var options = new JsonSerializerOptions();
                foreach (var v in vehicles)
                {
                    var jsonVehicle = JsonSerializer.Serialize(v, v.GetType(), options);
                    sw.WriteLine($"Type:{GetVehicleType(v)}");
                    sw.WriteLine(jsonVehicle);
                }
            }
                
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

        internal bool Load(string name, Func<int, bool> create, Func<IVehicle, bool> add)
        {
            string path = $"{Environment.CurrentDirectory}/{name}.txt";
            if(!File.Exists(path))
                throw new Exception("Name did not exist.");

            Name = name;

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

        private string GetVehicleType(IVehicle v)
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
            var capacity = config.GetSection(GarageSettings.PathCapacity).Value;
            return int.TryParse(capacity, out int res) ? res : 0;
        }
    }
}
