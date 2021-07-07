using Models.Enums;
using System;

namespace Models.Interfaces
{
    /// <summary>
    /// Indicators calculated from the market data.
    /// </summary>
    public interface ITechnicalAnalysis : IEquatable<ITechnicalAnalysis>
    {
        /// <summary>
        /// Fast Simple Moving Average.
        /// </summary>
        decimal FastSMA { get; set; }
        /// <summary>
        /// Slow Simple Moving Average.
        /// </summary>
        decimal SlowSMA { get; set; }
        /// <summary>
        /// Traders Action Zone. Determined by the slow and the fast moving average.
        /// </summary>
        TAZ TAZ { get; set; }
        /// <summary>
        /// Trend that may go up or down (or rarely can be undefined).
        /// </summary>
        Trend Trend { get; set; }
    }
}
