using System;

namespace DataVendor.Models
{
    /// <summary>
    /// The very basic data regarding one market data entity.
    /// </summary>
    public interface IMarketData
    {
        /// <summary>
        /// Latest recorded price (in euro). If recorded after closing it is called closing price.
        /// </summary>
        decimal ClosingPrice { get; set; }
        /// <summary>
        /// The datetime of the latest record.
        /// </summary>
        DateTime DateTime { get; set; }
        /// <summary>
        /// ISIN of the stock (ISIN = International Securities Identification Number).
        /// </summary>
        string Isin { get; set; }
        /// <summary>
        /// The number of stocks traded during the day.
        /// </summary>
        int Volumen { get; set; }
    }
}