using Peter.Models.Enums;
using System;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Basic stock data for one paper.
    /// </summary>
    public interface IRegistryEntry: IEquatable<IRegistryEntry>
    {
        /// <summary>
        /// International Securities Identification Number (unique ID).
        /// </summary>
        string Isin { get; set; }
        /// <summary>
        /// Name of the share.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Link to the investor info on the company's own site.
        /// </summary>
        string OwnInvestorLink { get; set; }
        /// <summary>
        /// Link to the investor info on the site of the registering stock exchange.
        /// </summary>
        string StockExchangeLink { get; set; }
        Position Position { get; set; }
        IFinancialReport FinancialReport { get; set; }
        IFinancialAnalysis FinancialAnalysis { get; set; }
    }
}