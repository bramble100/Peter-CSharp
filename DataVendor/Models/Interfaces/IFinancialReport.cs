using System;

namespace Models.Interfaces
{
    /// <summary>
    /// Quarterly reports related data and metadata.
    /// </summary>
    public interface IFinancialReport : IEquatable<IFinancialReport>
    {
        /// <summary>
        /// Earning Per Share.
        /// </summary>
        decimal EPS { get; }
        /// <summary>
        /// True if the report is outdated.
        /// </summary>
        bool IsOutdated { get; }
        /// <summary>
        /// The number of months covered by the financial report.
        /// </summary>
        int MonthsInReport { get; }
        /// <summary>
        /// The announced date on which the next financial report will be published. After this date the current report will be considered outdated.
        /// </summary>
        DateTime NextReportDate { get; }
    }
}
