using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    internal class MarketDataEntity : IMarketDataEntity
    {
        public decimal ClosingPrice { get; set; }
        public DateTime DateTime { get; set; }
        public string Isin { get; set; }
        public string Name { get; set; }
        public decimal PreviousDayClosingPrice { get; set; }
        public string StockExchange { get; set; }
        public int Volumen { get; set; }

        public int CompareTo(IMarketDataEntity other)
        {
            var result = Name.CompareTo(other.Name);
            return result != 0 ? result : DateTime.CompareTo(other.DateTime);
        }

        public override bool Equals(object obj) => Equals(obj as IMarketDataEntity);

        public bool Equals(IMarketDataEntity other) => other != null &&
                   ClosingPrice == other.ClosingPrice &&
                   DateTime == other.DateTime &&
                   Isin == other.Isin &&
                   Name == other.Name &&
                   Volumen == other.Volumen &&
                   PreviousDayClosingPrice == other.PreviousDayClosingPrice &&
                   StockExchange == other.StockExchange;

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

        public override string ToString() => 
            $"{Name}, " +
            $"{(string.IsNullOrEmpty(Isin) ? string.Empty : $"({Isin}), ")}" +
            $"Closing Price: {ClosingPrice}, " +
            $"DateTime: {DateTime}, " +
            $"Volumen: {Volumen}, " +
            $"Previous Day: {PreviousDayClosingPrice} " +
            $"({StockExchange})";
    }
}
