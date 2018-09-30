namespace Peter.Models.Interfaces
{
    /// <summary>
    /// The indicators and numbers calculated from the market data.
    /// </summary>
    public interface ITechnicalAnalysis
    {
        /// <summary>
        /// Fast Simple Moving Average.
        /// </summary>
        decimal FastSMA { get; set; }
        /// <summary>
        /// Slow Simple Moving Average.
        /// </summary>
        decimal SlowSMA { get; set; }
    }
}
