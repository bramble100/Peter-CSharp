using Peter.Models.Builders;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
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
            out IRegistryEntry result)
        {
            result = new RegistryEntry();

            if (input.Length != 8) return false;

            try
            {
                result = new RegistryEntry()
                {
                    Name = input[0],
                    Isin = input[1],
                    StockExchangeLink = input[2],
                    OwnInvestorLink = input[3],
                    FinancialReport = new FinancialReportBuilder()
                        .SetEPS(input[4])
                        .SetMonthsInReport(input[5])
                        .SetNextReportDate(input[6])
                        .Build()
                };
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string FormatForCSV(IRegistryEntry e, string separator, CultureInfo cultureInfo) => 
            string.Join(separator, new string[]
            {
                e.Name.WrapWithQuotes(),
                e.Isin,
                e.StockExchangeLink?.ToString(),
                e.OwnInvestorLink?.ToString(),
                e.FinancialReport?.EPS.ToString(cultureInfo).WrapWithQuotes(),
                e.FinancialReport?.MonthsInReport.ToString(),
                e.FinancialReport?.NextReportDate.ToString(cultureInfo),
                e.Position.ToString()
            });
    }
}
