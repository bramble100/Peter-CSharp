using System;

namespace Peter.Models.Interfaces
{
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
        /// All the metrics based on technical analysis.
        /// </summary>
        ITechnicalAnalysis TechnicalAnalysis { get; set; }
        /// <summary>
        /// All the metrics based on financial analysis.
        /// </summary>
        IFinancialAnalysis FinancialAnalysis { get; set; }
    }
}
