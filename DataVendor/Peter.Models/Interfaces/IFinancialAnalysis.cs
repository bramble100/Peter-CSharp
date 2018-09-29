namespace Peter.Models.Interfaces
{
    /// <summary>
    /// The indicators and numbers calculated from the market data.
    /// </summary>
    public interface IFinancialAnalysis
    {
        /// <summary>
        /// Latest closing price.
        /// </summary>
        decimal ClosingPrice { get; set; }
        /// <summary>
        /// Fast Simple Moving Average.
        /// </summary>
        decimal FastSMA { get; set; }
        /// <summary>
        /// Name of the share.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Price/Earning ratio (latest closing price divided by the earning per share).
        /// </summary>
        decimal PE { get; set; }
        /// <summary>
        /// Slow Simple Moving Average.
        /// </summary>
        decimal SlowSMA { get; set; }
    }
}
