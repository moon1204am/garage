using Garage.Model;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Garage.View
{
    internal class Validator
    {
        public string ValidateText(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? throw new ArgumentNullException() : input;
        }

        public int ValidateNumber(string text)
        {
            return int.TryParse(text, out var value) ? value : throw new ArgumentException("Not a number");
        }

        public double ValidateDouble(string text)
        {
            return double.TryParse(text, out var value) ? value : throw new ArgumentException("Not a number");
        }

        public bool ValidateLicenseNumber(string licenseNumber)
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

        public bool checkEmpty<T>(IEnumerable<T> e) where T : class
        {
            return !e.Any();
        }
    }
}
