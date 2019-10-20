using Peter.Models.Interfaces;
using System.Collections.Generic;

namespace Services.Analyses
{
    internal interface ITechnicalAnalyser
    {
        /// <summary>
        /// Creates a new technical analysis, based on closing price and base data.
        /// </summary>
        /// <param name="marketData"></param>
        /// <param name="fastMovingAverage"></param>
        /// <param name="slowMovingAverage"></param>
        /// <returns></returns>
        ITechnicalAnalysis NewAnalysis(
            IEnumerable<IMarketDataEntity> marketData, 
            int fastMovingAverageDayCount, 
            int slowMovingAverageDayCount);
    }
}