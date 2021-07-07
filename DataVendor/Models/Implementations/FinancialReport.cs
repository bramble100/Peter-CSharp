using Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Models.Implementations
{
    internal class FinancialReport : IFinancialReport
    {
        private readonly static HashSet<int> _validMonths = new HashSet<int>() { 3, 6, 9, 12 };

        public FinancialReport()
        {
        }

        public FinancialReport(decimal eps, int monthsInReport, DateTime nextReportDate) : this()
        {
            if (_validMonths.Contains(monthsInReport) && nextReportDate.Date >= DateTime.Now.AddDays(-7).Date)
            {
                EPS = eps;
                MonthsInReport = monthsInReport;
                NextReportDate = nextReportDate;
            }
        }

        public decimal EPS { get; private set; }
        public bool IsOutdated => NextReportDate.Date < DateTime.Now.Date;
        public int MonthsInReport { get; private set; }
        public DateTime NextReportDate { get; private set; } = DateTime.Now.Date;

        public bool Equals(IFinancialReport other) => other != null &&
                   EPS == other.EPS &&
                   MonthsInReport == other.MonthsInReport &&
                   NextReportDate == other.NextReportDate;

        public override bool Equals(object obj) => Equals(obj as FinancialReport);

        public override int GetHashCode()
        {
            var hashCode = -298693766;
            hashCode = hashCode * -1521134295 + EPS.GetHashCode();
            hashCode = hashCode * -1521134295 + MonthsInReport.GetHashCode();
            hashCode = hashCode * -1521134295 + NextReportDate.GetHashCode();
            return hashCode;
        }

        public override string ToString() => 
            $"EPS: {EPS} Months: {MonthsInReport} Next Report: {NextReportDate}";
    }
}
