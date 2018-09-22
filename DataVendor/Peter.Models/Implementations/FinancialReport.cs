using System;
using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class FinancialReport : IFinancialReport
    {
        public FinancialReport()
        {
        }

        public FinancialReport(decimal eps, int monthsInReport, DateTime nextReportDate)
        {
            if (new HashSet<int>() { 3, 6, 9, 12 }.Contains(monthsInReport) && nextReportDate > DateTime.Now)
            {
                EPS = eps;
                MonthsInReport = monthsInReport;
                NextReportDate = nextReportDate;
            }
        }

        public decimal EPS { get; private set; }
        public bool IsOutdated => NextReportDate > DateTime.Now;
        public int MonthsInReport { get; private set; }
        public DateTime NextReportDate { get; private set; } = DateTime.Now;
    }
}
