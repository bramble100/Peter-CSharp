using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Globalization;

namespace Peter.Models.Builders
{
    public class FinancialReportBuilder : IBuilder<IFinancialReport>
    {
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
            try
            {
                _monthsInReport = Convert.ToInt32(value);
                _monthsInReportSet = true;
            }
            catch (FormatException)
            {
            }
            return this;
        }

        public FinancialReportBuilder SetNextReportDate(string value)
        {
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

        public IFinancialReport Build()
        {
            return _EPSset && _monthsInReportSet && _nextReportDateSet 
                ? new FinancialReport(_eps, _monthsInReport, _nextReportDate) 
                : null;
        }
    }
}