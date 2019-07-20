using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineIsin
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

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
                _logger.Warn(ex, $"Line cannot be converted into name-to-isin entity ({string.Join(",", input)})");
                result = null;
                return false;
            }
        }

        public static string FormatForCSV(INameToIsin entity, string separator) => 
            string.Join(separator,
                entity.Name.WrapWithQuotes(),
                entity.Isin);
    }
}
