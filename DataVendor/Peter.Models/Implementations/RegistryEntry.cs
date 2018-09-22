using Peter.Models.Enums;
using Peter.Models.Interfaces;
using System;

namespace Peter.Models.Implementations
{
    public class RegistryEntry : IRegistryEntry
    {
        public string Name { get; set; }
        public Uri OwnInvestorLink { get; set; }
        public Uri StockExchangeLink { get; set; }
        public Position Position { get; set; }
        public IFinancialReport FinancialReport { get; set; }
        public IFinancialAnalysis FinancialAnalysis { get; set; }
    }
}
