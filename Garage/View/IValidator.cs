namespace Garage.View
{
    internal interface IValidator
    {
        bool CheckEmpty<T>(IEnumerable<T> e) where T : class;
        double ValidateDouble(string text);
        bool ValidateLicenseNumber(string licenseNumber);
        int ValidateNumber(string text);
        string? ValidateText(string input);
    }
}