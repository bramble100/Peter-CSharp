using Peter.Models.Builders;
using Peter.Models.Enums;
using Peter.Models.Interfaces;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Services.Analyses
{
    internal class TechnicalAnalyser : ITechnicalAnalyser
    {
        public ITechnicalAnalysis GetAnalysis(ImmutableArray<IMarketDataEntity> marketData, int fastMovingAverage, int slowMovingAverage)
        {
            var fastSMA = marketData.Take(fastMovingAverage).Average(d => d.ClosingPrice);
            var slowSMA = marketData.Take(slowMovingAverage).Average(d => d.ClosingPrice);

            var closingPrice = marketData.First().ClosingPrice;

            return new TechnicalAnalysisBuilder()
                .SetFastSMA(fastSMA)
                .SetSlowSMA(slowSMA)
                .SetTAZ(GetTAZ(closingPrice, fastSMA, slowSMA))
                .SetTrend(GetTrend(fastSMA, slowSMA))
                .Build();
        }

        internal static TAZ GetTAZ(decimal closingPrice, decimal fastSMA, decimal slowSMA)
        {
            if (closingPrice <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(closingPrice));
            if (fastSMA <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(fastSMA));
            if (slowSMA <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(slowSMA));

            if (closingPrice > Math.Max(fastSMA, slowSMA))
                return TAZ.AboveTAZ;
            if (closingPrice < Math.Min(fastSMA, slowSMA))
                return TAZ.BelowTAZ;

            return TAZ.InTAZ;
        }

        internal static Trend GetTrend(decimal fastSMA, decimal slowSMA)
        {
            if (fastSMA <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(fastSMA));
            if (slowSMA <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(slowSMA));

            if (fastSMA > slowSMA)
                return Trend.Up;
            if (fastSMA < slowSMA)
                return Trend.Down;

            return Trend.Undefined;
        }
    }
}