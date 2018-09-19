using System;
using System.Collections.Generic;
using System.Linq;

namespace Peter.Models.Validators
{
    internal static class Isin
    {
        public static bool TryParse(string[] input, out string name, out string isin)
        {
            name = string.Empty;
            isin = string.Empty;

            if(input.Count() != 2) return false;
            name = input[0];
            isin = input[1];
            return !string.IsNullOrWhiteSpace(name);
        }

        internal static bool TryParse(KeyValuePair<string, string> keyValuePair, out string name, out string isin)
        {
            name = keyValuePair.Key;
            isin = keyValuePair.Value;
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}
