using Garage.Model;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Garage.View
{
    /// <summary>
    /// Class responsible for validating user input and return values.
    /// </summary>
    internal class Validator
    {
        /// <summary>
        /// Validates if a string is null or whitespace.
        /// </summary>
        /// <param name="input">The string to validate.</param>
        /// <returns>the validated string</returns>
        /// <exception cref="ArgumentNullException">thrown if string is null or whitespace.</exception>
        public string ValidateText(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? throw new ArgumentNullException() : input;
        }

        /// <summary>
        /// Validates if a string is a valid integer.
        /// </summary>
        /// <param name="text">the string to validate.</param>
        /// <returns>the validated interger value of the string input.</returns>
        /// <exception cref="ArgumentException">thrown if parsing failed, meaning the input was not an integer.</exception>
        public int ValidateNumber(string text)
        {
            return int.TryParse(text, out var value) ? value : throw new ArgumentException("Not a number");
        }

        /// <summary>
        /// Validates if a string is a valid double.
        /// </summary>
        /// <param name="text">the string to validate.</param>
        /// <returns>the validated double value of the string input.</returns>
        /// <exception cref="ArgumentException">thrown if parsing failed.</exception>
        public double ValidateDouble(string text)
        {
            return double.TryParse(text, out var value) ? value : throw new ArgumentException("Not a number");
        }

        /// <summary>
        /// Validates whether a string is a valid Swedish license number.
        /// </summary>
        /// <param name="licenseNumber">the string license nr to validate.</param>
        /// <returns>true if valid, otherwise false.</returns>
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

        /// <summary>
        /// Generic helper function that checks if an IEnumerable of T or anything that implements the interface of T, is empty or not.
        /// </summary>
        /// <typeparam name="T">must be a reference type.</typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool checkEmpty<T>(IEnumerable<T> e) where T : class
        {
            return !e.Any();
        }
    }
}
