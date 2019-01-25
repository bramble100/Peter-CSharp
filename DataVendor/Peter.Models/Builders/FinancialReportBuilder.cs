using NLog;
using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Peter.Models.Builders
{
    public class FinancialReportBuilder : IBuilder<IFinancialReport>
    {
        private readonly static HashSet<int> _validMonths = new HashSet<int>() { 3, 6, 9, 12 };

        private readonly CultureInfo _cultureInfo;

        private bool _EPSset;
        private bool _monthsInReportSet;
        private bool _nextReportDateSet;

        private decimal _eps;
        private int _monthsInReport;
        private DateTime _nextReportDate;

        public FinancialReportBuilder()
        {
            _cultureInfo = CultureInfo.CurrentUICulture;
        }

        public FinancialReportBuilder(CultureInfo cultureInfo) : this()
        {
            _cultureInfo = cultureInfo;
        }

        public FinancialReportBuilder SetEPS(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return this;

            try
            {
                _eps = Convert.ToDecimal(value, _cultureInfo);
                _EPSset = true;
            }
            catch (FormatException)
            {
            }

            return this;
        }

        public FinancialReportBuilder SetMonthsInReport(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return this;

            try
            {
                _monthsInReport = Convert.ToInt32(value);
                _monthsInReportSet = _validMonths.Contains(_monthsInReport);
            }
            catch (FormatException)
            {
            }

            return this;
        }

        public FinancialReportBuilder SetNextReportDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return this;

            try
            {
                _nextReportDate = Convert.ToDateTime(value, _cultureInfo);
                _nextReportDateSet = true;
            }
            catch (FormatException)
            {
            }

            return this;
        }

        public IFinancialReport Build() => 
            _EPSset && _monthsInReportSet && _nextReportDateSet
                ? new FinancialReport(_eps, _monthsInReport, _nextReportDate)
                : null;
    }
}