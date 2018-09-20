using Peter.Models.Enums;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Class to hold all the basic stock data for one paper.
    /// </summary>
    public interface IRegistryEntry
    {
        /// <summary>
        /// Name of the share.
        /// </summary>
        string Name { get; set; }
        string OwnInvestorLink { get; set; }
        string StockExchangeLink { get; set; }
        Position Position { get; set; }
        IFinancialReport FinancialReport { get; set; }
        IFinancialAnalysis FinancialAnalysis { get; set; }
    }
}