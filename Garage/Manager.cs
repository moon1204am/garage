using Garage.Controller;
using Garage.Model;
using Garage.View;

namespace Garage
{
    internal class Manager
    {
        private IHandler handler;
        private IUI ui;
        private Validator util;
        private bool keepReceivingCmds;
        

        public Manager(IHandler handler, IUI ui) 
        {
            this.handler = handler;
            this.ui = ui;
            util = new Validator();
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
                else
                { 
                    ui.Print("Not a valid command. Try again.");
                    input = ui.GetInput();
                }
            }
            GetUserCommand();
        }

        private bool LoadGarage()
        {
            ui.Print("Enter garage name to load");
            var input = ui.GetInput().ToUpper();
            while(input != "QUIT")
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
                input = ui.GetInput().ToUpper();
            }
            return false;
        }

        public void GetUserCommand()
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

        private void SaveGarage()
        {
            if(!handler.IsLoaded) 
            {
                ui.Print("Please enter a name of the garage.");
                var name = ui.GetInput().ToUpper();
                bool saved = handler.Save(name);
                if (saved)
                {
                    ui.Print("Garage was saved.");
                    return;
                }
                ui.Print("Garage could not be saved.");
            }
        }

        private void GetParkedVehicles()
        {
            var parkedVehicles = handler.GetParkedVehicles();
            if (util.checkEmpty(parkedVehicles))
            {
                ui.Print("No vehicles parked yet.");
                return;
            }

            ui.Print("All parked vehicles right now:\n");
            foreach (var vehicle in parkedVehicles) ui.Print(vehicle.ToString()!);
        }

        private void GetTypesOfVehicles()
        {
            var types = handler.GetCountOfEachType();
            if(util.checkEmpty(types))
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
                
            IVehicle createdVehicle = GetVehicleSpecs()!;
            if(createdVehicle == null)
                return false;

            CreateVehicle(createdVehicle);
            return true;
        }

        private IVehicle? GetVehicleSpecs(bool isQuery = false)
        {
            var licenseNr = GetLicenseNumber(isQuery);
            if(!isQuery && licenseNr == "QUIT") return null;
            ui.Print("Enter colour");
            var colour = ui.GetInput().ToUpper();
            ui.Print("Enter nr of wheels");
            var nrOfWheels = ui.GetInput();
            IVehicle newVehicle = new Vehicle(licenseNr, colour, util.ValidateNumber(nrOfWheels));
            return newVehicle;
        }

        private string GetLicenseNumber(bool isQuery)
        {
            ui.Print("Enter license number");
            var licenseNr = ui.GetInput().ToUpper();
            if(isQuery) return licenseNr;
            while (licenseNr != "QUIT")
            {

                if(util.ValidateLicenseNumber(licenseNr) && !handler.LicenseAlreadyExists(licenseNr))
                    break;
                if (!util.ValidateLicenseNumber(licenseNr))
                    ui.Print("License number is invalid. Try again.");
                if (handler.LicenseAlreadyExists(licenseNr))
                    ui.Print("License nr already exists. Try again.");
                    
                ui.Print("Or type 'quit' to exit.");
                licenseNr = ui.GetInput().ToUpper();
            } 
            return licenseNr;
        }

        private VehicleType GetVehicleType()
        {
            string input = ui.GetInput();
            VehicleType parsedVehicle = Enum.TryParse(input.ToUpper(), out parsedVehicle) ? parsedVehicle : VehicleType.All;
            return parsedVehicle;
        }

        private void CreateVehicle(IVehicle vehicle)
        {
            ui.Print("Enter type of vehicle to park");
            ui.DisplayVehicleOptions();

            while (true)
            {
                switch (GetVehicleType())
                {
                    case VehicleType.Airplane:
                        CreateAirplane(vehicle);
                        return;
                    case VehicleType.Boat:
                        CreateBoat(vehicle);
                        return;
                    case VehicleType.Bus:
                        CreateBus(vehicle);
                        return;
                    case VehicleType.Car:
                        CreateCar(vehicle);
                        return;
                    case VehicleType.Motorcycle:
                        CreateMotorcycle(vehicle);
                        return;
                    default:
                        ui.Print("Please choose of the vehicles listed.");
                        break;
                }
            }
        }

        private void CreateMotorcycle(IVehicle vehicle)
        {
            ui.Print("Enter nr of seats");
            var nrOfSeats = ui.GetInput();
            var mc = vehicle as Motorcycle;
            mc.NrOfSeats = util.ValidateNumber(nrOfSeats);
            //mc = new Motorcycle(vehicle.LicenseNumber, vehicle.Colour, vehicle.NrOfWheels, util.ValidateNumber(nrOfSeats));
            if (handler.AddVehicle(vehicle))
                ui.Print($"Successfully parked {vehicle}");
            else
                throw new Exception($"Could not park {vehicle}");
        }

        private void CreateCar(IVehicle vehicle)
        {
            ui.Print("Enter cylinder volume");
            var cylinderVolume = ui.GetInput();
            IVehicle carToPark = new Car(vehicle.LicenseNumber, vehicle.Colour, vehicle.NrOfWheels, util.ValidateDouble(cylinderVolume));
            if (handler.AddVehicle(carToPark))
                ui.Print($"Successfully parked {carToPark}");
            else
                throw new Exception($"Could not park {carToPark}");
        }

        private void CreateBus(IVehicle vehicle)
        {
            ui.Print("Enter fuel type");
            var fuelType = ui.GetInput();
            IVehicle busToPark = new Bus(vehicle.LicenseNumber, vehicle.Colour, vehicle.NrOfWheels, fuelType);
            if (handler.AddVehicle(busToPark))
                ui.Print($"Successfully parked {busToPark}");
            else
                throw new Exception($"Could not park {busToPark}");
        }

        private void CreateBoat(IVehicle vehicle)
        {
            ui.Print("Enter length");
            var length = ui.GetInput();
            IVehicle boatToPark = new Boat(vehicle.LicenseNumber, vehicle.Colour, vehicle.NrOfWheels, util.ValidateDouble(length));
            if (handler.AddVehicle(boatToPark))
                ui.Print($"Successfully parked {boatToPark}");
            else
                throw new Exception($"Could not park {boatToPark}");
        }

        private void CreateAirplane(IVehicle vehicle)
        {
            ui.Print("Enter nr of engines");
            var nrOfEngines = ui.GetInput();
            IVehicle airplaneToPark = new Airplane(vehicle.LicenseNumber, vehicle.Colour, vehicle.NrOfWheels, util.ValidateNumber(nrOfEngines));
            if (handler.AddVehicle(airplaneToPark))
                ui.Print($"Successfully parked {airplaneToPark}");
            else
                throw new Exception($"Could not park {airplaneToPark}");
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
                IVehicle foundVehicle = handler.GetVehicleByLicense(licenseNr);
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
                int nrToPark = util.ValidateNumber(nr);
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

        public void NewGarage()
        {
            ui.Print("Enter capacity of garage");
            var capacity = ui.GetInput();
            while(true)
            {
                try
                {
                    ui.Print($"Creating garage with capacity {capacity}...");
                    if (handler.CreateGarage(util.ValidateNumber(capacity)))
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

            IVehicle vehicleQuery = GetVehicleSpecs(true)!;
            ui.DisplayVehicleOptions(true);
            VehicleType type = GetVehicleType();
            var result = handler.CustomQuery(vehicleQuery, type);
            if (!result.Any())
            {
                ui.Print("No result matched query.");
                return;
            }
            foreach (var r in result)
                ui.Print(r.ToString());
        }
    }
}
