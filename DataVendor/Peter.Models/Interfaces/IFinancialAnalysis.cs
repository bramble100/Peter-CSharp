using System;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// The indicators and numbers calculated from the market data.
    /// </summary>
    public interface IFinancialAnalysis : IEquatable<IFinancialAnalysis>
    {
        /// <summary>
        /// Price/Earning ratio (latest closing price divided by the earning per share).
        /// </summary>
        decimal PE { get; set; }
    }
}
