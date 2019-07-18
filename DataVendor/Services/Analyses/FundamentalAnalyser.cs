using Peter.Models.Builders;
using Peter.Models.Interfaces;

namespace Services.Analyses
{
    internal class FundamentalAnalyser : IFundamentalAnalyser
    {
        public IFundamentalAnalysis GetAnalysis(decimal closingPrice, IRegistryEntry stockBaseData) => 
            new FundamentalAnalysisBuilder()
                .SetClosingPrice(closingPrice)
                .SetEPS(stockBaseData.FinancialReport?.EPS)
                .SetMonthsInReport(stockBaseData.FinancialReport?.MonthsInReport)
                .Build();
    }
}