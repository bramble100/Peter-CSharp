using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class FinancialAnalysis : IFinancialAnalysis
    {
        private decimal _closingPrice;
        private decimal? _fastSMA;
        private decimal? _slowSMA;
        private decimal _pe;

        public bool Buyable { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal? FastSMA { get; set; }
        public decimal? SlowSMA { get; set; }
        public decimal PE { get; set; }
    }
}
