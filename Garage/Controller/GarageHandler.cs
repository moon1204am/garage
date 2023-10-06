using Garage.Model;
using Garage.View;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Garage.Controller
{
    /* En GarageHandler. För att abstrahera ett lager så att det inte finns någon direkt
     * kontakt mellan användargränssnittet och garage klassen. Detta görs lämpligen
     * genom en klass som hanterar funktionaliteten som gränssnittet behöver ha tillgång till.
    */
    public class GarageHandler : IHandler
    {
        Garage<IVehicle> garage = default!;
        public bool IsLoaded { get; set; }

        public bool AddVehicle(IVehicle vehicle)
        {
            return garage.Insert(vehicle);
        }

        public bool AddVehicles(IVehicle[] vehicles)
        {
            bool added = true;
            for (int i = 0; i < vehicles.Length; i++) 
            {
                added = AddVehicle(vehicles[i]);
                if (!added) added = false;
            }
            return added;
        }

        public bool CreateGarage(int capacity)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException();
            garage = new Garage<IVehicle>(capacity);
            return true;
        }

        public int CheckForFreeSpots() => garage.IsFull ? 0 : garage.GetFreeSpaces;

        public bool RemoveVehicle(string licenseNr)
        {
            var vehicleToRemove = GetVehicleByLicense(licenseNr);
            if(vehicleToRemove == null) return false;
            return garage.Remove(vehicleToRemove);
        }

        public bool LicenseAlreadyExists(string license) => garage.Any(v => v.LicenseNumber == license);

        public IVehicle GetVehicleByLicense(string licenseNumber) => garage.FirstOrDefault(v => v.LicenseNumber == licenseNumber);

        public IEnumerable<VehicleDTO> GetCountOfEachType()
        {
            var counts = garage.GroupBy(vehicle => vehicle.GetType().Name).Select(g => new VehicleDTO(g.Key, g.Count()));
            return counts;
        }

        public IEnumerable<IVehicle> GetParkedVehicles() => garage.Select(v => v);

        public IEnumerable<IVehicle> CustomQuery(IVehicle vehicle, VehicleType vehicleType)
        {
            IEnumerable<IVehicle> result = VehicleType.All == vehicleType ? garage : garage.Where(v => v.GetType().Name == Enum.GetName<VehicleType>(vehicleType));
            result = result.Where(v => v.LicenseNumber == (vehicle.LicenseNumber == "0" ? v.LicenseNumber : vehicle.LicenseNumber));
            result = result.Where(v => v.Colour == (vehicle.Colour == "0" ? v.Colour : vehicle.Colour));
            result = result.Where(v => v.NrOfWheels == (vehicle.NrOfWheels == 0 ? v.NrOfWheels : vehicle.NrOfWheels));

            return result;
        }

        public bool Save(string name)
        {
            if (garage.GetCount == 0 || NameExists(name))
                return false;

            var options = new JsonSerializerOptions();
            //var jsonString = JsonSerializer.Serialize(v, v.GetType(), options);

            using (StreamWriter sw = File.AppendText("garages.txt"))
            {
                sw.WriteLine($"Capacity:{garage.Capacity}");
                sw.WriteLine($"Name:{name}");
                foreach (var v in garage)
                {
                    //var jsonVehicle = JsonSerializer.Serialize(v);
                    //var jsonVehicle = JsonConvert.SerializeObject(v);
                    var jsonVehicle = JsonSerializer.Serialize(v, v.GetType(), options);
                    sw.WriteLine($"Type:{GetVehicleType(v)}");
                    sw.WriteLine(jsonVehicle);
                }
            }
            return true;
        }

        public bool Load(string name)
        {
            if (!NameExists(name))
                throw new Exception("Name did not exist");

            using (StreamReader reader = new StreamReader("garages.txt"))
            {
                var capacity = GetCapacity(reader, name);
                if (capacity < 1) return false;
                CreateGarage(capacity);
                string type = string.Empty;
                
                while (!reader.EndOfStream)
                {
                    var json = reader.ReadLine();
                    if (json.Contains("Capacity:"))
                    {
                        IsLoaded = true;
                        return true;
                    }
                    
                    if (json.Contains("Type:"))
                    {
                        string[] split = json.Split(":");
                        type = split[1];
                    }
                    json = reader.ReadLine();
                    if (!string.IsNullOrEmpty(type))
                    {
                        IVehicle? vehicleObj = null;
                        switch (type)
                        {
                            case "2":
                                //vehicleObj = JsonConvert.DeserializeObject<Airplane>(text);
                                vehicleObj = JsonSerializer.Deserialize<Airplane>(json);
                                break;
                            case "3":
                                //vehicleObj = JsonConvert.DeserializeObject<Boat>(text);
                                vehicleObj = JsonSerializer.Deserialize<Boat>(json);
                                break;
                            case "4":
                                //vehicleObj = JsonConvert.DeserializeObject<Bus>(text);
                                vehicleObj = JsonSerializer.Deserialize<Bus>(json);
                                break;
                            case "5":
                                //vehicleObj = JsonConvert.DeserializeObject<Car>(text);
                                vehicleObj = JsonSerializer.Deserialize<Car>(json);
                                break;
                            case "6":
                                //vehicleObj = JsonConvert.DeserializeObject<Motorcycle>(text);
                                vehicleObj = JsonSerializer.Deserialize<Motorcycle>(json);
                                break;
                        }
                        //var vehicleObj = JsonSerializer.Deserialize<Vehicle>(text);
                        //var vehicleObj = JsonConvert.DeserializeObject<IVehicle>(text);
                        garage.Insert(vehicleObj!);
                    }
                }
                return false;
            }
        }
        private bool NameExists(string nameToCheck)
        {
            using (StreamReader reader = new StreamReader("garages.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var text = reader.ReadLine();
                    if (text.Contains("Name:"))
                    {
                        string[] split = text.Split(":");
                        if (split[1] == nameToCheck) return true;
                    }
                }
            }
            return false;
        }

        private int GetCapacity(StreamReader reader, string name)
        {
            int capacity = -1;
            while(!reader.EndOfStream) 
            {
                var info = reader.ReadLine();
                if (info.Contains("Capacity"))
                {
                    string[] split = info.Split(':');
                    capacity = int.Parse(split[1]);
                    info = reader.ReadLine();
                    if (info.Contains($"Name:{name}"))
                    {
                        return capacity;
                    }
                }
            
                
            }
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
    }
}
