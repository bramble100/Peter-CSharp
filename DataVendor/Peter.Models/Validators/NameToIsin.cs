using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Peter.Models.Validators
{
    public static class NameToIsin
    {
        public static bool TryParse(
            string[] input, 
            out string name, 
            out string isin)
        {
            name = string.Empty;
            isin = string.Empty;

            if(input.Count() != 2) return false;
            name = input[0];
            isin = input[1];
            return !string.IsNullOrWhiteSpace(name) && Isin.IsValidOrEmpty(isin);
        }

        internal static bool TryParse(
            INameToIsin nameToIsin, 
            out string name, 
            out string isin)
        {
            name = nameToIsin.Name;
            isin = nameToIsin.Isin;
            return !string.IsNullOrWhiteSpace(name) && Isin.IsValidOrEmpty(isin);
        }
    }
}
