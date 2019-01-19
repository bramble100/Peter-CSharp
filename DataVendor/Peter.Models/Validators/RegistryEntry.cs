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
            output = null;

            if (string.IsNullOrWhiteSpace(input))
                return true;

            if (!Uri.IsWellFormedUriString(input, UriKind.Absolute))
                return false;

            output = new Uri(input, UriKind.Absolute);
            return true;
        }
    }
}
