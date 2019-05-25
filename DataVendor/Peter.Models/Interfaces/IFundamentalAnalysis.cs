using System;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Indicators calculated from the fundamental numbers and market data.
    /// </summary>
    public interface IFundamentalAnalysis : IEquatable<IFundamentalAnalysis>
    {
        /// <summary>
        /// Price/Earning ratio (latest closing price divided by the earning per share).
        /// </summary>
        decimal PE { get; set; }
    }
}
