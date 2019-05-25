using Peter.Models.Interfaces;
using System.Collections.Immutable;

namespace Services.Analyses
{
    internal interface IFundamentalAnalyser
    {
        IFundamentalAnalysis GetAnalysis(
            decimal closingPrice, 
            IRegistryEntry stockBaseData);
    }
}