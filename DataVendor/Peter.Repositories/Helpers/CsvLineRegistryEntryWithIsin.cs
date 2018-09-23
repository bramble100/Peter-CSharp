using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineRegistryEntryWithIsin
    {
        public static bool TryParseFromCsv(string[] input, out KeyValuePair<string, IRegistryEntry> result)
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
                    OwnInvestorLink = new Uri(input[6]),
                    StockExchangeLink = new Uri(input[5]),
                    FinancialReport = new FinancialReport(
                        Convert.ToDecimal(input[2], new CultureInfo("en-US")),
                        Convert.ToInt32(input[3]),
                        Convert.ToDateTime(input[4], CultureInfo.InvariantCulture))
                });
                return true;
            }
            catch (Exception)
            {
                result = new KeyValuePair<string, IRegistryEntry>();
                return false;
            }
        }

        public static string FormatForCSV(KeyValuePair<string, IRegistryEntry> e, string separator) => 
            string.Join(separator, new string[]
            {
                e.Value.Name,
                e.Key,
                e.Value.FinancialReport.EPS.ToString(new CultureInfo("us-EN")),
                e.Value.FinancialReport.MonthsInReport.ToString(),
                e.Value.FinancialReport.NextReportDate.ToString(new CultureInfo("us-EN")),
                e.Value.StockExchangeLink.ToString(),
                e.Value.OwnInvestorLink.ToString(),
                e.Value.Position.ToString()
            });
    }
}
