using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class FinancialAnalysis : IFinancialAnalysis
    {
        public bool Buyable => throw new System.NotImplementedException();

        public decimal ClosingPrice { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public decimal? FastSMA { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public decimal? SlowSMA { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public decimal PE { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
