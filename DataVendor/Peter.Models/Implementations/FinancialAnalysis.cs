using Peter.Models.Enums;
using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class FinancialAnalysis : IFinancialAnalysis
    {
        private decimal _closingPrice;
        private decimal? _fastSMA;
        private decimal? _slowSMA;
        private decimal _pe;

        public decimal ClosingPrice { get; set; }
        public decimal FastSMA { get; set; }
        public string Name { get; set; }
        public decimal PE { get; set; }
        public decimal SlowSMA { get; set; }
        public TAZ TAZ { get; set; }
    }
}
