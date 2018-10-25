using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Repositories.Helpers
{
    public static class Extensions
    {
        private static readonly CultureInfo culture = new CultureInfo("hu-HU");

        public static IMarketDataEntity ParserFromCSV(this IEnumerable<string> strings)
        {
            var queue = new Queue<string>(strings);

            return new MarketDataEntity
            {
                Name = queue.Dequeue(),
                Isin = queue.Dequeue(),
                ClosingPrice = Convert.ToDecimal(queue.Dequeue(), culture),
                DateTime = Convert.ToDateTime(queue.Dequeue(), culture),
                Volumen = Convert.ToInt32(queue.Dequeue()),
                PreviousDayClosingPrice = Convert.ToDecimal(queue.Dequeue(), culture),
                StockExchange = queue.Dequeue()
            };
        }

        /// <summary>
        /// Returns a formatted string for writing into CSV file.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatterForCSV(this IMarketDataEntity entity, string separator)
        {
            return string.Join(separator,
                entity.Name.WrapWithQuotes(),
                entity.Isin,
                entity.ClosingPrice.ToString(culture),
                entity.DateTime.ToString(culture),
                entity.Volumen.ToString(culture),
                entity.PreviousDayClosingPrice.ToString(culture),
                entity.StockExchange);
        }

        public static string FormatterForCSV(this KeyValuePair<string, string> kvp, string separator)
        {
            return string.Join(separator,
                kvp.Key.WrapWithQuotes(),
                kvp.Value);
        }

        public static string WrapWithQuotes(this object obj) => $"\"{obj.ToString()}\"";
    }
}
