using Peter.Models.Implementations;
using Peter.Models.Interfaces;
using System;

namespace Peter.Models.Builders
{
    public class TechnicalAnalysisBuilder : IBuilder<ITechnicalAnalysis>
    {
        private readonly ITechnicalAnalysis _technicalAnalysis;
        private bool _fastSMAset;
        private bool _slowSMAset;

        public TechnicalAnalysisBuilder()
        {
            _technicalAnalysis = new TechnicalAnalysis();
        }

        public TechnicalAnalysisBuilder SetFastSMA(decimal value)
        {
            _technicalAnalysis.FastSMA = value > 0 ? value : throw new ArgumentOutOfRangeException("Fast SMA must be positive.");
            _fastSMAset = true;
            return this;
        }

        public TechnicalAnalysisBuilder SetSlowSMA(decimal value)
        {
            _technicalAnalysis.SlowSMA = value > 0 ? value : throw new ArgumentOutOfRangeException("Slow SMA must be positive.");
            _slowSMAset = true;
            return this;
        }

        public ITechnicalAnalysis Build() => _fastSMAset && _slowSMAset ? _technicalAnalysis : null;
    }
}