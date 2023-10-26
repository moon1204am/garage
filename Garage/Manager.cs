using Garage.Controller;
using Garage.Model;
using Garage.View;

namespace Garage
{
    /// <summary>
    /// Manages the applications UI and IHandler. All calls to the handler goes through this class.
    /// </summary>
    internal class Manager
    {
        private IHandler handler;
        private IUI ui;
        private IValidator validator;
        private bool keepReceivingCmds;

        public Manager(IHandler handler, IUI ui, IValidator validator) 
        {
            this.handler = handler;
            this.ui = ui;
            this.validator = validator;
        }

        public void Start()
        {
            while(true)
            {
                ui.Display();
                var input = ui.GetInput();
                if (input == "1")
                {
                    NewGarage();
                    break;
                }
                else if (input == "2")
                {
                    if(LoadGarage())
                    {
                        ui.Print("Garage successfully loaded");
                        break;
                    }  
                }
                else if (input == "3")
                {
                    if(handler.CreateGarageFromConfig())
                    {
                        ui.Print("Created garage with value used from configuration file.");
                        break;
                    }
                }
                else
                { 
                    ui.Print("Not a valid command. Try again.");
                    input = ui.GetInput();
                }
            }
            GetUserCommand();
        }

        private void GetUserCommand()
        {
            keepReceivingCmds = true;
            string input;
            while (keepReceivingCmds)
            {
                ui.DisplayCommands();
                input = ui.GetInput();
                try
                {
                    switch (input)
                    {
                        case "1":
                            GetParkedVehicles();
                            break;
                        case "2":
                            GetTypesOfVehicles();
                            break;
                        case "3":
                            ParkVehicle();
                            break;
                        case "4":
                            CheckoutVehicle();
                            break;
                        case "5":
                            NewGarage();
                            break;
                        case "6":
                            ParkVehicles();
                            break;
                        case "7":
                            FindVehicleByLicense();
                            break;
                        case "8":
                            CustomQuery();
                            break;
                        case "0":
                            SaveGarage();
                            keepReceivingCmds = false;
                            break;
                        default:
                            ui.Print("Illegal command.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void GetParkedVehicles()
        {
            var parkedVehicles = handler.GetParkedVehicles();
            if (validator.CheckEmpty(parkedVehicles))
            {
                ui.Print("No vehicles parked yet.");
                return;
            }

            ui.Print("All parked vehicles right now:\n");
            foreach (var vehicle in parkedVehicles) 
            { 
                ui.Print($"{vehicle.ToString()!}\n"); 
            }
        }

        private void GetTypesOfVehicles()
        {
            var types = handler.GetCountOfEachType();
            if(validator.CheckEmpty(types))
            {
                ui.Print("No vehicle type is parked yet.");
                return;
            }
            ui.Print("Number of each vehicle type parked right now:\n");
            foreach (var v in types)
                Console.WriteLine("Vehicle type = {0}, Nr parked = {1}", v.Type, v.NrOfType);
        }

        private bool ParkVehicle()
        {
            if (handler.CheckForFreeSpots() == 0)
                throw new Exception("Parking garage is full.");

            VehicleType type = GetVehicle();
            IVehicle createdVehicle = GetVehicleSpecs(type)!;
            if (createdVehicle == null) return false;
            if (handler.AddVehicle(createdVehicle))
            {
                ui.Print($"Successfully parked {createdVehicle}");
                return true;
            }
            else 
                throw new Exception($"Could not park {createdVehicle}");
        }

        private IVehicle? GetVehicleSpecs(VehicleType type = VehicleType.All, bool isQuery = false)
        {
            var licenseNr = GetLicenseNumber(isQuery);
            if(!isQuery && licenseNr == "QUIT") return null;
            ui.Print("Enter colour");
            var colour = validator.ValidateText(ui.GetInput().ToUpper());
            while (colour == null) {
                ui.Print("Please enter a valid color");
                colour = validator.ValidateText(ui.GetInput().ToUpper());
            }
            ui.Print("Enter nr of wheels");
            var nrOfWheels = validator.ValidateNumber(ui.GetInput());
            while (nrOfWheels == -1)
            {
                ui.Print("Please enter a valid nr");
                nrOfWheels = validator.ValidateNumber(ui.GetInput());
            }
            return GetTypeToCreate(type, licenseNr, colour, nrOfWheels);
        }

        private IVehicle? GetTypeToCreate(VehicleType type, string licenseNr, string colour, int nrOfWheels)
        {
            IVehicle vehicleToCreate = null!;
            if(type.Equals(VehicleType.Airplane)) 
            {
                ui.Print("Enter nr of engines");
                var nrOfEngines = validator.ValidateNumber(ui.GetInput());
                while(nrOfEngines == -1)
                {
                    ui.Print("Please enter a valid nr");
                    nrOfEngines = validator.ValidateNumber(ui.GetInput());
                }
                vehicleToCreate = new Airplane(licenseNr, colour, nrOfWheels, nrOfEngines);
                return vehicleToCreate;
            }
            else if(type.Equals(VehicleType.Boat))
            {
                ui.Print("Enter length");
                var length = validator.ValidateDouble(ui.GetInput());
                while (length == -1)
                {
                    ui.Print("Please enter a valid nr");
                    length = validator.ValidateNumber(ui.GetInput());
                }
                vehicleToCreate = new Boat(licenseNr, colour, nrOfWheels, length);
                return vehicleToCreate;
            }
            else if (type.Equals(VehicleType.Bus))
            {
                ui.Print("Enter fuel type");
                ui.DisplayFuelTypes();
                var fuelType = ui.GetInput();
                while (true)
                {
                    if (fuelType == "1")
                    {
                        fuelType = "DIESEL";
                        break;
                    }
                    else if (fuelType == "2")
                    {
                        fuelType = "GASOLINE";
                        break;
                    }
                    else
                    {
                        ui.Print("Please choose between the alternatives.");
                        fuelType = ui.GetInput();
                    }
                }
                vehicleToCreate = new Bus(licenseNr, colour, nrOfWheels, fuelType);
                return vehicleToCreate;
            }
            else if (type.Equals(VehicleType.Car))
            {
                ui.Print("Enter cylinder volume");
                var cylinderVolume = validator.ValidateDouble(ui.GetInput());
                while (cylinderVolume == -1)
                {
                    ui.Print("Please enter a valid nr");
                    cylinderVolume = validator.ValidateNumber(ui.GetInput());
                }
                vehicleToCreate = new Car(licenseNr, colour, nrOfWheels, cylinderVolume);
                return vehicleToCreate;
            }
            else if (type.Equals(VehicleType.Motorcycle))
            {
                ui.Print("Enter nr of seats");
                var nrOfSeats = validator.ValidateNumber(ui.GetInput());
                while (nrOfSeats == -1)
                {
                    ui.Print("Please enter a valid nr");
                    nrOfSeats = validator.ValidateNumber(ui.GetInput());
                }
                vehicleToCreate = new Motorcycle(licenseNr, colour, nrOfWheels, nrOfSeats);
                return vehicleToCreate;
            }
            else if(type.Equals(VehicleType.All))
            {
                vehicleToCreate = new Vehicle(licenseNr, colour, nrOfWheels);
            }
            return vehicleToCreate;
        }

        private string GetLicenseNumber(bool isQuery)
        {
            ui.Print("Enter license number");
            var licenseNr = ui.GetInput().ToUpper();
            if(isQuery) return licenseNr;
            while (licenseNr != "QUIT")
            {

                if(validator.ValidateLicenseNumber(licenseNr) && !handler.LicenseAlreadyExists(licenseNr))
                    break;
                if (!validator.ValidateLicenseNumber(licenseNr))
                    ui.Print("License number is invalid. Try again.");
                if (handler.LicenseAlreadyExists(licenseNr))
                    ui.Print("License nr already exists. Try again.");
                    
                ui.Print("Or type 'quit' to exit.");
                licenseNr = ui.GetInput().ToUpper();
            } 
            return licenseNr;
        }

        private VehicleType GetVehicle()
        {
            ui.Print("Enter type of vehicle to park");
            ui.DisplayVehicleOptions();

            while (true)
            {
                switch (GetVehicleType())
                {
                    case VehicleType.Airplane:
                        return VehicleType.Airplane;
                    case VehicleType.Boat:
                        return VehicleType.Boat;
                    case VehicleType.Bus:
                        return VehicleType.Bus;
                    case VehicleType.Car:
                        return VehicleType.Car;
                    case VehicleType.Motorcycle:
                        return VehicleType.Motorcycle;
                    default:
                        ui.Print("Please choose of the vehicles listed.");
                        break;
                }
            }
        }

        private VehicleType GetVehicleType()
        {
            string input = ui.GetInput();
            VehicleType parsedVehicle = Enum.TryParse(input.ToUpper(), out parsedVehicle) ? parsedVehicle : VehicleType.All;
            return parsedVehicle;
        }

        private void CheckoutVehicle()
        {
            ui.Print("Enter your license plate nr please.");
            string licenseNr = ui.GetInput().ToUpper();
            while(licenseNr != "QUIT")
            {
                if (handler.RemoveVehicle(licenseNr))
                {
                    ui.Print($"Successfully checked out vehicle with license nr {licenseNr}");
                    break;
                }
                ui.Print($"Vehicle with license nr {licenseNr} did not exist in the garage. Try again or type 'quit' to exit.");
                licenseNr = ui.GetInput().ToUpper();
            }
        }

        private void FindVehicleByLicense()
        {
            ui.Print("Enter license number: ");
            string licenseNr = ui.GetInput().ToUpper();
            while (licenseNr != "QUIT")
            {
                IVehicle? foundVehicle = handler.GetVehicleByLicense(licenseNr);
                if (foundVehicle != null)
                {
                    ui.Print($"Vehicle with license nr {licenseNr} = {foundVehicle.ToString()}");
                    break;
                }
                ui.Print("No vehicle found with given license number. Try again.");
                ui.Print("Or type 'quit' to exit.");

                licenseNr = ui.GetInput().ToUpper();
            }
        }

        private void ParkVehicles()
        {
            ui.Print("How many vehicles do you want to add?");
            var nr = ui.GetInput();
            while (nr != "QUIT")
            {
                int nrToPark = validator.ValidateNumber(nr);
                if (nrToPark > 1)
                {
                    for (int i = 0; i < nrToPark; i++)
                    {
                        if (!ParkVehicle()) return;
                    }
                }
                ui.Print("You need to park atleast 2 vehicles. Try again. Or type 'quit' to exit.");
                nr = ui.GetInput().ToUpper();
            }
        }

        private void NewGarage()
        {
            ui.Print("Enter capacity of garage");
            var capacity = ui.GetInput();
            while(true)
            {
                try
                {
                    ui.Print($"Creating garage with capacity {capacity}...");
                    if (handler.CreateGarage(validator.ValidateNumber(capacity)))
                    {
                        ui.Print("Garage created.");
                        break;
                    }
                } catch(Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentException)
                {
                    ui.Print($"Failed to create garage with capacity {capacity}. Try again.");
                }
                capacity = ui.GetInput();
            }
        }
        private void CustomQuery()
        {
            ui.Print("Write '0' on properties you DON'T want to query on.");

            IVehicle vehicleQuery = GetVehicleSpecs(isQuery: true)!;
            ui.DisplayVehicleOptions(true);
            VehicleType type = GetVehicleType();
            var result = handler.CustomQuery(vehicleQuery, type);
            if (!result.Any())
            {
                ui.Print("No result matched query.");
                return;
            }
            foreach (var r in result)
                ui.Print("{r.ToString()!\n}");
        }

        private void SaveGarage()
        {
            if (!handler.IsLoaded && handler.GetCount > 0)
            {
                ui.Print("Please enter a name of the garage.");
                var name = ui.GetInput().ToLower();
                bool saved = handler.Save(name);
                if (saved)
                {
                    ui.Print("Garage was saved.");
                    return;
                }
                ui.Print("Garage could not be saved.");
            }
            else if(handler.IsLoaded)
            {
                ui.Print("Enter name of garage to save it.");
                var name = ui.GetInput().ToLower();
                while (true)
                {
                    if (handler.Update(name)) 
                    { 
                        ui.Print("Garage successfully updated."); 
                        break; 
                    }
                    ui.Print("Please enter name of correct garage.");
                    name = ui.GetInput().ToLower();
                }
                
            }
        }

        private bool LoadGarage()
        {
            ui.Print("Enter garage name to load");
            var input = ui.GetInput().ToLower();
            while (input != "quit")
            {
                try
                {
                    return handler.Load(input);
                }
                catch (Exception e)
                {
                    ui.Print(e.Message);
                    ui.Print("Try again or type 'quit' to exit.");
                }
                input = ui.GetInput().ToLower();
            }
            return false;
        }
    }
}
