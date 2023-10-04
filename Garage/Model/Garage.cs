using Garage.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Model
{
    public class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        private T[] vehicles;
        private int currentIndex;
        private int count;
        public int Capacity { get; }

        public Garage(int capacity)
        {
            Capacity = capacity;
            vehicles = new T[capacity];
            currentIndex = 0;
            count = 0;
        }

        public bool Insert(T vehicle)
        {
            if(IsFull()) return false;
            else if (currentIndex == (Capacity - 1) && vehicles[currentIndex] != null)
            {
                int index = SearchForFreeSpace();
                if (index > -1) vehicles[index] = vehicle;
            } 
            else
            {
                vehicles[currentIndex] = vehicle;
                if (currentIndex < Capacity - 1) currentIndex++;
            }

            //printing to see if it inserted
            //foreach (var v in vehicles)
            //{
            //    v.ToString();
            //}
            count++;
            return true;
        }

        internal bool Remove(string licenseNr)
        {
            int index = Array.FindIndex(vehicles, vehicle => vehicle != null && vehicle.LicenseNumber == licenseNr);
            if (index == -1) return false;
            vehicles[index] = default(T)!;
            count--;
            return true;
        }

        private int SearchForFreeSpace()
        {
            for(int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i] != null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool IsFull()
        {
            return GetFreeSpaces() == 0;
        }

        public int GetCount()
        {
            return count;
        }
        
        public int GetFreeSpaces()
        {
            return Capacity - GetCount();
        }

        public bool LicenseAlreadyExists(string license)
        {
            return vehicles.Any(v => (v != null) && v.LicenseNumber.Equals(license));
        }

        public IVehicle RetrieveVehicleByLicense(string licenseNumber)
        {
            return vehicles.Single(v => v != null && v.LicenseNumber == licenseNumber);
        }
        public string GetCountOfEachType()
        {

            //IEnumerable<string> types;
            var airplanes = vehicles.Where(v => v != null && v.GetType().Name == "Airplane").Count();
            var boats = vehicles.Where(v => (v != null) && v.GetType().Name == "Boat").Count();
            var buses = vehicles.Where(v => v != null && v.GetType().Name == "Bus").Count();
            var cars = vehicles.Where(v => v != null && v.GetType().Name == "Car").Count();
            var motorcycles = vehicles.Where(v => v != null && v.GetType().Name == "Motorcycle").Count();


            string res = $"Airplanes = {airplanes}\nBoats = {boats}\nBuses = {buses}\nCars = {cars}\nMotorcycles = {motorcycles}";
            return res;

            //Dictionary<string, int> counts = vehicles.GroupBy(vehicle => vehicle.GetType().Name).Where(vehicle => vehicle != null).ToDictionary(k => k.GetType().Name, v => v.Count());
            var counts = vehicles.GroupBy(vehicle => vehicle.GetType().Name).Select(g => new VehicleDTO2(g.Key, g.Count()));
            
            //return counts;
            
        }

        public IEnumerable<T> GetAllVehicles()
        {
            return vehicles.Where(v => v != null);
            //var result = vehicles.SelectMany(v => v.LicenseNumber, (parent, child) => $"{parent.LicenseNumber} {parent.Colour} {parent.NrOfWheels} {child.ToString()}").FirstOrDefault();
            //return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            
            foreach (T v in vehicles)//.EmptyIfNull() /*?? Enumerable.Empty<T>()*/)
            {
             if(v is not null)   
                yield return v;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        
    }
}
