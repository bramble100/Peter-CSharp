using Models.Builders;
using Models.Interfaces;

namespace Services.Analyses
{
    internal class FundamentalAnalyser : IFundamentalAnalyser
    {
        /// <summary>
        /// Creates a new fundamental analysis, based on closing price and base data.
        /// </summary>
        /// <param name="closingPrice"></param>
        /// <param name="stockBaseData"></param>
        /// <returns></returns>
        public IFundamentalAnalysis NewAnalysis(decimal closingPrice, IRegistryEntry stockBaseData) => 
            new FundamentalAnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetEPS(stockBaseData?.FinancialReport?.EPS)
                .SetMonthsInReport(stockBaseData?.FinancialReport?.MonthsInReport)
                .Build();
    }
}