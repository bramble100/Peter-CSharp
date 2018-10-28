using Peter.Models.Interfaces;
using System;

namespace Peter.Models.Implementations
{
    public class FinancialAnalysis : IFinancialAnalysis
    {
        public FinancialAnalysis()
        {
        }

        public FinancialAnalysis(decimal closingPrice, decimal? eps) : this()
        {
            if (eps == null || eps == 0) return;
            PE = Math.Round(closingPrice / (decimal)eps, 1);
        }

        public decimal PE { get; set; }
    }
}
