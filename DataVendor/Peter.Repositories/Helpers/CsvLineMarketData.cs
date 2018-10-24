using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineMarketData
    {
        public static string[] Header => new string[]
        {
            "Name",
            "ISIN",
            "Closing Price",
            "DateTime",
            "Volumen",
            "Previous Day Closing Price",
            "Stock Exchange"
        };

        /// <summary>
        /// Returns a formatted string for writing into CSV file.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="separator"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string FormatForCSV(IMarketDataEntity entity, string separator, CultureInfo cultureInfo) =>
            string.Join(separator,
                entity.Name.WrapWithQuotes(),
                entity.Isin,
                entity.ClosingPrice.ToString(cultureInfo),
                entity.DateTime.ToString(cultureInfo),
                entity.Volumen.ToString(cultureInfo),
                entity.PreviousDayClosingPrice.ToString(cultureInfo),
                entity.StockExchange);

        public static bool TryParseFromCsv(string[] input, CultureInfo cultureInfo, out IMarketDataEntity result)
        {
            try
            {
                result = new MarketDataEntity
                {
                    Name = input[0],
                    Isin = input[1],
                    ClosingPrice = Convert.ToDecimal(input[2], cultureInfo),
                    DateTime = Convert.ToDateTime(input[3], cultureInfo),
                    Volumen = Convert.ToInt32(input[4]),
                    PreviousDayClosingPrice = Convert.ToDecimal(input[5], cultureInfo),
                    StockExchange = input[6]
                };
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Warn(ex, $"Line cannot be converted into market data entity ({string.Join(",", input)})");
                result = null;
                return false;
            }
        }
    }
}
