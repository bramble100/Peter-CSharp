using Models.Enums;
using Models.Implementations;
using Models.Interfaces;
using System;

namespace Models.Builders
{
    public class TechnicalAnalysisBuilder : IBuilder<ITechnicalAnalysis>
    {
        private bool _fastSMAset;
        private bool _slowSMAset;

        private decimal _fastSMA;
        private decimal _slowSMA;

        private TAZ _taz;
        private Trend _trend;

        public TechnicalAnalysisBuilder SetFastSMA(decimal value)
        {
            if (value > 0)
            {
                _fastSMA = value;
                _fastSMAset = true;
            }

            return this;
        }

        public TechnicalAnalysisBuilder SetSlowSMA(decimal value)
        {
            if (value > 0)
            {
                _slowSMA = value;
                _slowSMAset = true;
            }

            return this;
        }

        public TechnicalAnalysisBuilder SetTAZ(string value)
        {
            if (Enum.TryParse<TAZ>(value, true, out var result)) _taz = result;

            return this;
        }

        public TechnicalAnalysisBuilder SetTAZ(TAZ value)
        {
            _taz = value;

            return this;
        }

        public TechnicalAnalysisBuilder SetTrend(string value)
        {
            if (Enum.TryParse<Trend>(value, true, out var result)) _trend = result;

            return this;
        }

        public TechnicalAnalysisBuilder SetTrend(Trend value)
        {
            _trend = value;

            return this;
        }

        public ITechnicalAnalysis Build() => 
            _fastSMAset && _slowSMAset 
            ? new TechnicalAnalysis()
            {
                FastSMA = _fastSMA,
                SlowSMA = _slowSMA,
                TAZ = _taz,
                Trend = _trend
            } 
            : null;
    }
}