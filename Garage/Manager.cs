using Garage.Controller;
using Garage.Model;
using Garage.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal class Manager
    {
        private IHandler handler;
        ConsoleUI ui;
        private Util util;
        private bool keepReceivingCmds;
        

        public Manager(IHandler handler) 
        {
            this.handler = handler;
            ui = new ConsoleUI();
            util = new Util();
        }
        public void Start()
        {
            ui.Display();
            NewGarage();
            GetUserCommand();
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
            if (parkedVehicles.Count() == 0)
            {
                ui.Print("No vehicles parked yet.");
                return;
            }

            ui.Print("All parked vehicles right now:\n");
            foreach (var vehicle in parkedVehicles) ui.Print(vehicle.ToString()!);
        }

        private void GetTypesOfVehicles()
        {
            ui.Print("Number of each vehicle type parked right now:\n");
            ui.Print(handler.GetVehicleTypesCount());
            //var types = handler.GetVehicleTypesCount();
            //foreach (var kvp in types)
            //{
            //    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //}
        }

        private void ParkVehicle()
        {
            if (handler.CheckForFreeSpots() == 0)
                throw new Exception("Parking garage is full.");

            //REFACTOR INPUT TO REUSE METHOD THAT RETURNS LICENSE NR, COLOUR, NR OF WHEELS
            ui.Print("Enter license number");
            var licenseNr = ui.GetInput().ToUpper();

            while (!util.ValidateLicenseNumber(licenseNr) || !handler.LicenseIsUnique(licenseNr))
            {
                ui.Print("License number is either invalid or it already exists. Try again.");
                licenseNr = ui.GetInput().ToUpper();
            }

            ui.Print("Enter colour");
            var colour = ui.GetInput();
            ui.Print("Enter nr of wheels");
            var nrOfWheels = ui.GetInput();
            CreateVehicle(licenseNr, colour, nrOfWheels);
        }

        private void CreateVehicle(string licenseNr, string colour, string nrOfWheels)
        {
            string input;
            ui.Print("Enter type of vehicle to park");
            ui.DisplayVehicleOptions();

            while (true)
            {
                input = ui.GetInput();
                switch (input)
                {
                    case "1":
                        CreateAirplane(licenseNr, colour, nrOfWheels);
                        return;
                    case "2":
                        CreateBoat(licenseNr, colour, nrOfWheels);
                        return;
                    case "3":
                        CreateBus(licenseNr, colour, nrOfWheels);
                        return;
                    case "4":
                        CreateCar(licenseNr, colour, nrOfWheels);
                        return;
                    case "5":
                        CreateMotorcycle(licenseNr, colour, nrOfWheels);
                        return;
                    default:
                        ui.Print("Please choose of the vehicles listed.");
                        break;
                }
            }

        }

        private void CreateMotorcycle(string licenseNr, string colour, string nrOfWheels)
        {
            ui.Print("Enter nr of seats");
            var nrOfSeats = ui.GetInput();
            IVehicle motorCycleToPark = new Motorcycle(licenseNr, colour, util.ValidateNumber(nrOfWheels), util.ValidateNumber(nrOfSeats));
            if (handler.AddVehicle(motorCycleToPark))
                ui.Print($"Successfully parked {motorCycleToPark}");
            else
                throw new Exception($"Could not park {motorCycleToPark}");
        }

        private void CreateCar(string licenseNr, string colour, string nrOfWheels)
        {
            ui.Print("Enter cylinder volume");
            var cylinderVolume = ui.GetInput();
            IVehicle carToPark = new Car(licenseNr, colour, util.ValidateNumber(nrOfWheels), util.ValidateDouble(cylinderVolume));
            if (handler.AddVehicle(carToPark))
                ui.Print($"Successfully parked {carToPark}");
            else
                throw new Exception($"Could not park {carToPark}");
        }

        private void CreateBus(string licenseNr, string colour, string nrOfWheels)
        {
            ui.Print("Enter fuel type");
            var fuelType = ui.GetInput();
            IVehicle busToPark = new Bus(licenseNr, colour, util.ValidateNumber(nrOfWheels), fuelType);
            if (handler.AddVehicle(busToPark))
                ui.Print($"Successfully parked {busToPark}");
            else
                throw new Exception($"Could not park {busToPark}");
        }

        private void CreateBoat(string licenseNr, string colour, string nrOfWheels)
        {
            ui.Print("Enter length");
            var length = ui.GetInput();
            IVehicle boatToPark = new Boat(licenseNr, colour, util.ValidateNumber(nrOfWheels), util.ValidateDouble(length));
            if (handler.AddVehicle(boatToPark))
                ui.Print($"Successfully parked {boatToPark}");
            else
                throw new Exception($"Could not park {boatToPark}");
        }

        private void CreateAirplane(string licenseNr, string colour, string nrOfWheels)
        {
            ui.Print("Enter nr of engines");
            var nrOfEngines = ui.GetInput();
            IVehicle airplaneToPark = new Airplane(licenseNr, colour, util.ValidateNumber(nrOfWheels), util.ValidateNumber(nrOfEngines));
            if (handler.AddVehicle(airplaneToPark))
                ui.Print($"Successfully parked {airplaneToPark}");
            else
                throw new Exception($"Could not park {airplaneToPark}");
        }

        private void CheckoutVehicle()
        {
            ui.Print("Enter your license plate nr please.");
            string licenseNr = ui.GetInput().ToUpper();
            if (!util.ValidateLicenseNumber(licenseNr))
                throw new Exception("Not a valid license nr");
            if (handler.RemoveVehicle(licenseNr))
                ui.Print($"Successfully checked out vehicle with license nr {licenseNr}");
            else
            {
                throw new Exception("Vehicle did not exist in the garage.");
            }

        }

        private void FindVehicleByLicense()
        {
            ui.Print("Enter license number: ");
            string licenseNr = ui.GetInput().ToUpper();
            try
            {
                IVehicle foundVehicle = handler.GetVehicleByLicense(licenseNr);
                ui.Print($"Vehicle with license nr {licenseNr} = {foundVehicle.ToString}");
            }
            catch (Exception)
            {
                ui.Print("No vehicle found with given license number");
            }
        }

        private void ParkVehicles()
        {
            ui.Print("How many vehicles do you want to add?");
            var nr = ui.GetInput();
            int nrToPark = util.ValidateNumber(nr);
            if (nrToPark < 2) throw new Exception();
            for (int i = 0; i < nrToPark; i++)
            {
                ParkVehicle();
            }
        }

        public void NewGarage()
        {
            ui.Print("Enter capacity of garage");
            var capacity = ui.GetInput();

            ui.Print($"Creating garage with capacity {capacity}...");
            handler.CreateGarage(util.ValidateNumber(capacity));
            ui.Print("Garage created.");
        }
        private void CustomQuery()
        {
            bool isDone = false;
            while (!isDone)
            {
                //vehicle
                //vehicle and color
                //vehicle and wheels
                //vehicle and color and wheels


                ui.Print("Choose query type" +
                    "\n1. Vehicle" +
                    "\n2. Vehicle and colour" +
                    "\n3. Vehicle and wheels" +
                    "\n4. Vehicle and color and wheels");

                int query = util.ValidateNumber(ui.GetInput());

                ui.Print("Enter vehicle to query on");
                ui.DisplayVehicleOptions(true);

                if (query == 1)
                {

                }
                else if (query == 2)
                {

                }
                else if (query == 3)
                {

                }
                else if (query == 4)
                {

                }

                ui.Print("Enter colour to query on");
                string colour = ui.GetInput();
                ui.Print("Enter nr of wheels to query on");
                int nrOfWheels = util.ValidateNumber(ui.GetInput());

            }
        }
    }
}
