using Peter.Models.Interfaces;
using System.Collections.Immutable;

namespace Services.Analyses
{
    internal interface ITechnicalAnalyser
    {
        ITechnicalAnalysis GetAnalysis(
            ImmutableArray<IMarketDataEntity> marketData, 
            int fastMovingAverage, 
            int slowMovingAverage);
    }
}