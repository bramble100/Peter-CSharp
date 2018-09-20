using System;

namespace Peter.Models.Validators
{
    public static class RegistryEntry
    {
        public static bool TryParseName(string input, out string output)
        {
            var isValid = !string.IsNullOrWhiteSpace(input);
            output = isValid ? input.Trim() : string.Empty;
            return isValid;
        }

        public static bool TryParseLink(string input, out Uri output)
        {
            try
            {
                output = new Uri(input.Trim());
                return true;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }
    }
}
