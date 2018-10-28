using Peter.Models.Builders;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineRegistryEntryWithIsin
    {
        public static string[] Header => new string[]
        {
                "Name",
                "ISIN",
                "Stock Exchange Link",
                "Own Investor Link",
                "EPS",
                "Months in Report",
                "Next Report Date",
                "Position"
        };

        public static bool TryParseFromCsv(
            string[] input, 
            CultureInfo cultureInfo, 
            out KeyValuePair<string, IRegistryEntry> result)
        {
            if (input.Length != 8)
            {
                result = new KeyValuePair<string, IRegistryEntry>();
                return false;
            }
            try
            {
                result = new KeyValuePair<string, IRegistryEntry>(input[1], new RegistryEntry
                {
                    Name = input[0],
                    StockExchangeLink = input[2],
                    OwnInvestorLink = input[3],
                    FinancialReport = new FinancialReportBuilder()
                        .SetEPS(input[4])
                        .SetMonthsInReport(input[5])
                        .SetNextReportDate(input[6])
                        .Build()
                });
                return true;
            }
            catch (Exception)
            {
                result = new KeyValuePair<string, IRegistryEntry>();
                return false;
            }
        }

        public static string FormatForCSV(KeyValuePair<string, IRegistryEntry> e, string separator, CultureInfo cultureInfo) => 
            string.Join(separator, new string[]
            {
                e.Value.Name.WrapWithQuotes(),
                e.Key,
                e.Value.StockExchangeLink?.ToString(),
                e.Value.OwnInvestorLink?.ToString(),
                e.Value.FinancialReport?.EPS.ToString(cultureInfo).WrapWithQuotes(),
                e.Value.FinancialReport?.MonthsInReport.ToString(),
                e.Value.FinancialReport?.NextReportDate.ToString(cultureInfo),
                e.Value.Position.ToString()
            });
    }
}
