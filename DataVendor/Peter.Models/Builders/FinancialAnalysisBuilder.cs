using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;

namespace Peter.Models.Builders
{
    public class FinancialAnalysisBuilder : IBuilder<IFinancialAnalysis>
    {
        private readonly IFinancialAnalysis _financialAnalysis;
        private bool _closingPriceSet;
        private bool _EPSset;

        private decimal _closingPrice;
        private decimal _eps;

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
            if (value.HasValue)
            {
                _eps = value.Value;
                _EPSset = true;
            }
            return this;
        }

        public IFinancialAnalysis Build()
        {
            return _closingPriceSet && _EPSset ? _financialAnalysis : null;
        }
    }
}