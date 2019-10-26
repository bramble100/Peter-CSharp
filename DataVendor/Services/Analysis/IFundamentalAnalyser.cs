using Models.Interfaces;

namespace Services.Analysis
{
    public interface IFundamentalAnalyser
    {
        /// <summary>
        /// Creates a new fundamental analysis, based on closing price and base data.
        /// </summary>
        /// <param name="closingPrice"></param>
        /// <param name="stockBaseData"></param>
        /// <returns></returns>
        IFundamentalAnalysis NewAnalysis(decimal closingPrice, IRegistryEntry stockBaseData);
    }
}