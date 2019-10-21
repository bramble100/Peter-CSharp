using Models.Enums;
using System;

namespace Models.Interfaces
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
        Uri OwnInvestorLink { get; set; }
        /// <summary>
        /// Link to the investor info on the site of the registering stock exchange.
        /// </summary>
        Uri StockExchangeLink { get; set; }
        /// <summary>
        /// Financial report. It is based on the report issued quarterly by the company.
        /// </summary>
        IFinancialReport FinancialReport { get; set; }
        /// <summary>
        /// Fundamental Analysis. It is based on the numbers in the quarterly financial report.
        /// </summary>
        IFundamentalAnalysis FundamentalAnalysis { get; set; }
    }
}