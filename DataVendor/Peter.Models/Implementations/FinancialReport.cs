using System;
using System.Collections.Generic;
using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class FinancialReport : IFinancialReport
    {
        private readonly static HashSet<int> _validMonths = new HashSet<int>() { 3, 6, 9, 12 };

        public FinancialReport()
        {
        }

        public FinancialReport(decimal eps, int monthsInReport, DateTime nextReportDate)
        {
            if (_validMonths.Contains(monthsInReport) && nextReportDate.Date >= DateTime.Now.AddYears(-5).Date)
            {
                EPS = eps;
                MonthsInReport = monthsInReport;
                NextReportDate = nextReportDate;
            }
        }

        public decimal EPS { get; private set; }
        public bool IsOutdated => NextReportDate < DateTime.Now.Date;
        public int MonthsInReport { get; private set; }
        public DateTime NextReportDate { get; private set; } = DateTime.Now.Date;

        public bool Equals(IFinancialReport other)
        {
            return other != null &&
                   EPS == other.EPS &&
                   MonthsInReport == other.MonthsInReport &&
                   NextReportDate == other.NextReportDate;
        }

        public override bool Equals(object obj) => Equals(obj as FinancialReport);

        public override int GetHashCode()
        {
            var hashCode = -298693766;
            hashCode = hashCode * -1521134295 + EPS.GetHashCode();
            hashCode = hashCode * -1521134295 + MonthsInReport.GetHashCode();
            hashCode = hashCode * -1521134295 + NextReportDate.GetHashCode();
            return hashCode;
        }
    }
}
