﻿namespace Garage.Model
{
    /// <summary>
    /// Class representing a Bus.
    /// </summary>
    public class Bus : Vehicle
    {
        public string FuelType { get; }
        public Bus(string licenseNumber, string colour, int nrOfWheels, string fuelType) : base(licenseNumber, colour, nrOfWheels)
        {
            FuelType = fuelType;
        }

        public override string ToString() => $"{base.ToString()}\nFuel type: {FuelType}";
    }
}
