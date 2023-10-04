using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Garage.View
{
    internal class Util
    {
        public void ValidateText(string input)
        {
            throw new NotImplementedException();
        }

        public int ValidateNumber(string text)
        {
            return int.TryParse(text, out var value) ? value : throw new Exception("Not a number");
        }

        public double ValidateDouble(string text)
        {
            return double.TryParse(text, out var value) ? value : throw new Exception("Not a number");
        }

        internal bool ValidateLicenseNumber(string licenseNumber)
        {
            // ^ start of string
            // [A-Z] a letter
            // {3} 3 times
            // [0-9] a number
            // {2} 2 times
            // [A-Z - 0-9] a letter or a number
            // {1} one time
            // $ end of string
            Regex regex = new Regex("^[A-Za-z]{3}[0-9]{2}[A-Za-z0-9]{1}$");
            return regex.IsMatch(licenseNumber);
        }
    }
}
