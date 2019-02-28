using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineIsin
    {
        public static string[] Header => new string[]        
        {
            "Name",
            "ISIN"
        };

        public static bool TryParseFromCsv(string[] input, out INameToIsin result)
        {
            try
            {
                result = new NameToIsin(input[0], input[1]);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Warn(ex, $"Line cannot be converted into name-to-isin entity ({string.Join(",", input)})");
                result = new NameToIsin(string.Empty, string.Empty);
                return false;
            }
        }

        public static string FormatForCSV(INameToIsin entity, string separator) => 
            string.Join(separator,
                entity.Name.WrapWithQuotes(),
                entity.Isin);
    }
}
