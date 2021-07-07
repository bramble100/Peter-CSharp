using System.Linq;

namespace Models.Validators
{
    public static class Isin
    {
        public static bool IsValid(string input) => 
            !string.IsNullOrWhiteSpace(input) && IsValidOrEmpty(input);

        public static bool IsValidOrEmpty(string input) =>
            string.IsNullOrWhiteSpace(input) || (
            input.Length == 12 &&
            input.All(c => char.IsLetterOrDigit(c))) &&
            input.Substring(0,2).All(c => char.IsLetter(c));
    }
}
