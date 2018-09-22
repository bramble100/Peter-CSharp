using Peter.Models.Interfaces;
using System.Collections.Generic;

namespace Peter.Repositories.Helpers
{
    public class CsvLineRegistryEntryWithIsin
    {
        public static bool TryParseFromCsv(string[] input, out KeyValuePair<string, IRegistryEntry> result)
        {
            result = new KeyValuePair<string, IRegistryEntry>();

            if (input.Length != 7) return false;

            return true;
        }
    }
}
