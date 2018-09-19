using System;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Stores the data of an entity downloaded from the data vendor page.
    /// </summary>
    public interface IMarketDataEntity : IComparable<IMarketDataEntity>, IEquatable<IMarketDataEntity>
    {
        decimal ClosingPrice { get; set; }
        DateTime DateTime { get; set; }
        string Isin { get; set; }
        string Name { get; set; }
        /// <summary>
        /// Recorded closing price (in euro) on the previous day.
        /// </summary>
        decimal PreviousDayClosingPrice { get; set; }
        /// <summary>
        /// The name of the stock exchange from where the data were downloaded.
        /// </summary>
        string StockExchange { get; set; }
        int Volumen { get; set; }

        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}