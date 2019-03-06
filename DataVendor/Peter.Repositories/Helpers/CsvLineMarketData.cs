using NLog;
using Peter.Models.Builders;
using Peter.Models.Interfaces;
using System;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class CsvLineMarketData
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

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
            result = new MarketDataEntityBuilder()
                .SetName(input[0])
                .SetIsin(input[1])
                .SetClosingPrice(Convert.ToDecimal(input[2], cultureInfo))
                .SetDateTime(Convert.ToDateTime(input[3], cultureInfo))
                .SetVolumen(Convert.ToInt32(input[4]))
                .SetPreviousDayClosingPrice(Convert.ToDecimal(input[5], cultureInfo))
                .SetStockExchange(input[6])
                .Build();

            if(result is null)
            {
                _logger.Warn($"Line cannot be converted into market data entity ({string.Join(",", input)})");
            }

            return result != null;
        }
    }
}
