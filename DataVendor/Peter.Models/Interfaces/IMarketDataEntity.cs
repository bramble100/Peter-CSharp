using System;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Stores the data of an entity downloaded from the data vendor page.
    /// </summary>
    public interface IMarketDataEntity : IComparable<IMarketDataEntity>, IEquatable<IMarketDataEntity>
    {
        /// <summary>
        /// Latest recorded price (in euro). If recorded after closing it is called closing price.
        /// </summary>
        decimal ClosingPrice { get; set; }
        DateTime DateTime { get; set; }
        /// <summary>
        /// ISIN of the stock (ISIN = International Securities Identification Number).
        /// </summary>
        string Isin { get; set; }
        /// <summary>
        /// Name of the stock.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Recorded closing price (in euro) on the previous day.
        /// </summary>
        decimal PreviousDayClosingPrice { get; set; }
        /// <summary>
        /// The name of the stock exchange from where the data were downloaded.
        /// </summary>
        string StockExchange { get; set; }
        /// <summary>
        /// Number of stocks traded during the day.
        /// </summary>
        int Volumen { get; set; }
    }
}