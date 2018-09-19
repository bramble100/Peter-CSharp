using System;

namespace Peter.Models.Interfaces
{
    public interface IMarketDataEntity : IComparable<IMarketDataEntity>, IEquatable<IMarketDataEntity>
    {
        decimal ClosingPrice { get; set; }
        DateTime DateTime { get; set; }
        string Isin { get; set; }
        string Name { get; set; }
        decimal PreviousDayClosingPrice { get; set; }
        string StockExchange { get; set; }
        int Volumen { get; set; }

        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}