namespace Models.Validators
{
    public static class RegistryEntry
    {
        public static bool TryParseName(string input, out string output)
        {
            var isValid = !string.IsNullOrWhiteSpace(input);
            output = isValid ? input.Trim() : string.Empty;
            return isValid;
        }
    }
}
