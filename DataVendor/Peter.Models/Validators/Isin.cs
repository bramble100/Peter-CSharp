using System.Linq;

namespace Peter.Models.Validators
{
    public static class Isin
    {
        public static bool IsValid(string input) => !string.IsNullOrWhiteSpace(input) && IsValidOrEmpty(input);

        public static bool IsValidOrEmpty(string input) =>
            string.IsNullOrWhiteSpace(input) || (
            input.Length == 12 &&
            input.All(c => char.IsLetterOrDigit(c)));
    }
}
