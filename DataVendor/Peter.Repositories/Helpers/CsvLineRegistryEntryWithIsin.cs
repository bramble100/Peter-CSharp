using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineRegistryEntryWithIsin
    {
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
                FinancialReport report = null;
                try
                {
                    if(!(string.IsNullOrWhiteSpace(input[4]) && 
                        string.IsNullOrWhiteSpace(input[5]) &&
                        string.IsNullOrWhiteSpace(input[6])))
                    {
                        report = new FinancialReport(
                            Convert.ToDecimal(input[4], cultureInfo),
                            Convert.ToInt32(input[5]),
                            Convert.ToDateTime(input[6], cultureInfo));
                    }
                }
                catch
                {
                }
                result = new KeyValuePair<string, IRegistryEntry>(input[1], new RegistryEntry
                {
                    Name = input[0],
                    StockExchangeLink = input[2],
                    OwnInvestorLink = input[3],
                    FinancialReport = report
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
