using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineFinancialAnalysis
    {
        public static string[] Header => new string[]
        {
            "Name",
            "ISIN",
            "Closing Price",
            "Price/Earning"
        };

        public static string FormatForCSV(KeyValuePair<string, IFinancialAnalysis> e, string separator, CultureInfo cultureInfo) =>
                string.Join(
                    separator,
                    new string[]
                    {
                        e.Value.Name.WrapWithQuotes(),
                        e.Key,
                        e.Value.ClosingPrice.WrapWithQuotes(),
                        e.Value.PE.WrapWithQuotes()
                    });
    }
}
