using Garage.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Garage.Controller
{
    /// <summary>
    /// Class that is responsible for saving new and loading existing garages.
    /// </summary>
    public class Utilities
    {
        IConfiguration config;
        //private int rowToInsert;

        public Utilities(IConfiguration config)
        {
            this.config = config;
        }

        public bool IsLoaded { get; set; }

        public bool Save(string name, int capacity, IEnumerable<IVehicle> parkedVehicles)
        {
            var nameExists = GetRowFromName(name);
            if (nameExists.nameExists)
                return false;

            var options = new JsonSerializerOptions();

            using (StreamWriter sw = File.AppendText("garages.txt"))
            {
                sw.WriteLine($"Capacity:{capacity}");
                sw.WriteLine($"Name:{name}");
                foreach (var v in parkedVehicles)
                {
                    var jsonVehicle = JsonSerializer.Serialize(v, v.GetType(), options);
                    sw.WriteLine($"Type:{GetVehicleType(v)}");
                    sw.WriteLine(jsonVehicle);
                }
            }
            return true;
        }

        public bool Load(string name, Func<int, bool> create, Func<IVehicle, bool> add)
        {
            var nameExistsWhere = GetRowFromName(name);
            if (!nameExistsWhere.nameExists)
                throw new Exception("Name did not exist");

            using (StreamReader reader = new StreamReader("garages.txt"))
            {
                //rowToInsert = nameExistsWhere.row;
                var capacity = GetCapacity(reader, name, nameExistsWhere.row - 1);
                if (capacity < 1) return false;
                create(capacity);
                string type = string.Empty;

                while (!reader.EndOfStream)
                {
                    var json = reader.ReadLine();
                    //rowToInsert++;
                    if (json.Contains("Capacity:"))
                    {
                        IsLoaded = true;
                        return true;
                    }
                    if (json.Contains("Type:"))
                    {
                        string[] split = json.Split(":");
                        type = split[1];
                        json = reader.ReadLine();
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

        public (bool nameExists, int row) GetRowFromName(string nameToCheck)
        {
            using (StreamReader reader = new StreamReader("garages.txt"))
            {
                int row = 0;
                while (!reader.EndOfStream)
                {
                    var text = reader.ReadLine();
                    row++;
                    if (text.Contains("Name:"))
                    {
                        string[] split = text.Split(":");
                        if (split[1] == nameToCheck) return (true, row);
                    }
                }
            }
            return (false, -1);
        }

        private int GetCapacity(StreamReader reader, string name, int row)
        {
            int capacity = -1;
            string info = string.Empty;
            for (int i = 0; i < row; i++)
                info = reader.ReadLine();

            string[] split = info.Split(':');
            capacity = int.Parse(split[1]);

            return capacity;
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
            var capacity = config.GetSection("garage:garagesettings:capacity").Value;
            return int.TryParse(capacity, out int res) ? res : 0;

        }

        //internal bool Update(string name)
        //{
        //    var options = new JsonSerializerOptions();
        //    string text = string.Empty;
        //    using(StreamReader reader = new StreamReader("garages.txt"))
        //    {
        //        for (int i = 0; i < rowToInsert; i++)
        //        {
        //            text = reader.ReadLine();
        //        }
        //    }

        //    using (StreamWriter sw = File.AppendText("garages.txt"))
        //    {
                

        //        var jsonVehicle = JsonSerializer.Serialize(v, v.GetType(), options);
        //        sw.WriteLine($"Type:{GetVehicleType(v)}");
        //        sw.WriteLine(jsonVehicle);
        //    }
        //    return true;
        //}
    }
}
