using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Peter.Models.Builders
{
    public class FinancialAnalysisBuilder : IBuilder<IFinancialAnalysis>
    {
        private static HashSet<int> ValidMonthsInReportValues = new HashSet<int>() { 3, 6, 9, 12 };

        private readonly IFinancialAnalysis _financialAnalysis;
        private bool _closingPriceSet;
        private bool _EPSset;
        private bool _monthsInReportSet;

        private decimal _closingPrice;
        private decimal _eps;
        private int _monthsInReport;

        public FinancialAnalysisBuilder()
        {
            _financialAnalysis = new FinancialAnalysis();
        }

        public FinancialAnalysisBuilder SetClosingPrice(decimal value)
        {
            _closingPrice = value > 0 ? value : throw new ArgumentOutOfRangeException("Closing price must be positive.");
            _closingPriceSet = true;
            return this;
        }

        public FinancialAnalysisBuilder SetEPS(decimal? value)
        {
            if (value.HasValue && value.Value != 0)
            {
                _eps = value.Value;
                _EPSset = true;
            }
            return this;
        }

        public FinancialAnalysisBuilder SetMonthsInReport(int? value)
        {
            if (value.HasValue && ValidMonthsInReportValues.Contains(value.Value))
            {
                _monthsInReport = value.Value;
                _monthsInReportSet = true;
            }
            return this;
        }

        public IFinancialAnalysis Build()
        {
            if (!_closingPriceSet || !_EPSset || !_monthsInReportSet)
            {
                return null;
            }

            _financialAnalysis.PE = Math.Round(_closingPrice / _eps / _monthsInReport * 12, 2);
            return _financialAnalysis;
        }
    }
}