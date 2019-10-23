using Models.Builders;
using Models.Enums;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Services.Analyses
{
    public class TechnicalAnalyser : ITechnicalAnalyser
    {
        /// <summary>
        /// Creates a new technical analysis, based on market data.
        /// </summary>
        /// <param name="marketData"></param>
        /// <param name="fastMovingAverageDayCount"></param>
        /// <param name="slowMovingAverageDayCount"></param>
        /// <returns></returns>
        public ITechnicalAnalysis NewAnalysis(IEnumerable<IMarketDataEntity> marketData, int fastMovingAverageDayCount, int slowMovingAverageDayCount)
        {
            var marketDataArray = marketData.OrderByDescending(item => item.DateTime).ToImmutableArray();
            var fastSMA = marketDataArray.Take(fastMovingAverageDayCount).Average(d => d.ClosingPrice);
            var slowSMA = marketDataArray.Take(slowMovingAverageDayCount).Average(d => d.ClosingPrice);

            var closingPrice = marketDataArray.First().ClosingPrice;

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