using NLog;
using Peter.Models.Builders;
using Peter.Models.Interfaces;
using System;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineRegistryEntryWithIsin
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public static string[] Header => new string[]
        {
                "Name",
                "ISIN",
                "Stock Exchange Link",
                "Own Investor Link",
                "EPS",
                "Months in Report",
                "Next Report Date"
        };

        public static bool TryParseFromCsv(
            string[] input,
            CultureInfo cultureInfo,
            out IRegistryEntry result)
        {
            result = null;

            try
            {
                if (input.Length != 7
                    || string.IsNullOrWhiteSpace(input[0])
                    || string.IsNullOrWhiteSpace(input[1]))
                    return false;

                result = new RegistryEntryBuilder()
                    .SetName(input[0])
                    .SetIsin(input[1])
                    .SetStockExchangeLink(input[2])
                    .SetOwnInvestorLink(input[3])
                    .SetFinancialReport(new FinancialReportBuilder()
                        .SetEPS(input[4])
                        .SetMonthsInReport(input[5])
                        .SetNextReportDate(input[6])
                        .Build())
                    .Build();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string FormatForCSV(IRegistryEntry entry, string separator, CultureInfo cultureInfo)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
            if (string.IsNullOrEmpty(separator))
                throw new ArgumentNullException(nameof(separator));
            if (cultureInfo is null)
                throw new ArgumentNullException(nameof(cultureInfo));

            return string.Join(separator, new string[]
            {
                entry.Name.WrapWithQuotes(),
                entry.Isin,
                entry.StockExchangeLink?.ToString(),
                entry.OwnInvestorLink?.ToString(),
                entry.FinancialReport?.EPS.ToString(cultureInfo).WrapWithQuotes(),
                entry.FinancialReport?.MonthsInReport.ToString(),
                entry.FinancialReport?.NextReportDate.ToString(cultureInfo),
            });
        }
    }
}
