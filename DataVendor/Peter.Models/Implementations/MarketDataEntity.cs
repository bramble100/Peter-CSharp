using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    /// <summary>
    /// Stores the data of an entity downloaded from the data vendor page.
    /// </summary>
    public class MarketDataEntity : IMarketDataEntity
    {
        public decimal ClosingPrice { get; set; }
        public DateTime DateTime { get; set; }
        public string Isin { get; set; }
        public string Name { get; set; }
        public int Volumen { get; set; }
        /// <summary>
        /// Recorded closing price (in euro) on the previous day.
        /// </summary>
        public decimal PreviousDayClosingPrice { get; set; }
        /// <summary>
        /// The name of the stock exchange from where the data were downloaded.
        /// </summary>
        public string StockExchange { get; set; }

        public int CompareTo(IMarketDataEntity other)
        {
            var result = Name.CompareTo(other.Name);
            return result != 0 ? result : DateTime.CompareTo(other.DateTime);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => Equals(obj as IMarketDataEntity);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(IMarketDataEntity other)
        {
            return other != null &&
                   ClosingPrice == other.ClosingPrice &&
                   DateTime == other.DateTime &&
                   Isin == other.Isin &&
                   Name == other.Name &&
                   Volumen == other.Volumen &&
                   PreviousDayClosingPrice == other.PreviousDayClosingPrice &&
                   StockExchange == other.StockExchange;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 378868840;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + ClosingPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + DateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + Volumen.GetHashCode();
            hashCode = hashCode * -1521134295 + PreviousDayClosingPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StockExchange);
            return hashCode;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => 
            $"{Name}, " +
            $"{(String.IsNullOrEmpty(Isin) ? String.Empty : $"ISIN: {Isin}, ")}" +
            $"Closing Price: {ClosingPrice}, " +
            $"DateTime: {DateTime}, " +
            $"Volumen: {Volumen}, " +
            $"Previous Day: {PreviousDayClosingPrice} " +
            $"({StockExchange})";
    }
}
