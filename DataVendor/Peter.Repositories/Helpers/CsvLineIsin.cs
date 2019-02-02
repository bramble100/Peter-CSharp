using NLog;
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

        public static bool TryParseFromCsv(string[] input, out KeyValuePair<string, string> result)
        {
            try
            {
                result = new KeyValuePair<string, string>(input[0], input[1]);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Warn(ex, $"Line cannot be converted into name-to-isin entity ({string.Join(",", input)})");
                result = new KeyValuePair<string, string>(string.Empty, string.Empty);
                return false;
            }
        }

    }
}
