using Peter.Models.Interfaces;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineAnalysis
    {
        public static string[] Header => new string[]
        {
            "Name",
            "ISIN",
            "Closing Price",
            "Price/Earning",
            "TAZ Status",
            "Trend",
            "Qty In Buying Packet"
        };

        public static string FormatForCSV(KeyValuePair<string, IAnalysis> e, string separator, CultureInfo cultureInfo) =>
                string.Join(
                    separator,
                    new string[]
                    {
                        e.Value.Name.WrapWithQuotes(),
                        e.Key,
                        e.Value.ClosingPrice.WrapWithQuotes(),
                        e.Value.FinancialAnalysis?.PE.WrapWithQuotes(),
                        e.Value.TechnicalAnalysis?.TAZ.ToString(),
                        e.Value.TechnicalAnalysis?.Trend.ToString(),
                        e.Value.QtyInBuyingPacket.ToString()
                    });
    }
}
