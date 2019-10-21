using System;

namespace Models.Interfaces
{
    /// <summary>
    /// All-in analysis containing some basic data with financial and technical analyses.
    /// </summary>
    public interface IAnalysis : IEquatable<IAnalysis>
    {
        /// <summary>
        /// Name of the share.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Latest closing price.
        /// </summary>
        decimal ClosingPrice { get; set; }
        /// <summary>
        /// The number of stocks to buy in one packet (for preset amount of money).
        /// </summary>
        int QtyInBuyingPacket { get; set; }
        /// <summary>
        /// Indicators based on technical analysis.
        /// </summary>
        ITechnicalAnalysis TechnicalAnalysis { get; set; }
        /// <summary>
        /// Indicators based on financial analysis.
        /// </summary>
        IFundamentalAnalysis FundamentalAnalysis { get; set; }
    }
}
