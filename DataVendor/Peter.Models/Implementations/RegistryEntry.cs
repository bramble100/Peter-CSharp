using Peter.Models.Enums;
using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

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

        public bool Equals(IRegistryEntry other)
        {
            return other != null &&
                   Name == other.Name &&
                   EqualityComparer<Uri>.Default.Equals(OwnInvestorLink, other.OwnInvestorLink) &&
                   EqualityComparer<Uri>.Default.Equals(StockExchangeLink, other.StockExchangeLink) &&
                   Position == other.Position &&
                   EqualityComparer<IFinancialReport>.Default.Equals(FinancialReport, other.FinancialReport) &&
                   EqualityComparer<IFinancialAnalysis>.Default.Equals(FinancialAnalysis, other.FinancialAnalysis);
        }

        public override bool Equals(object obj) => Equals(obj as IRegistryEntry);

        public override int GetHashCode()
        {
            var hashCode = 741974547;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(OwnInvestorLink);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(StockExchangeLink);
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IFinancialReport>.Default.GetHashCode(FinancialReport);
            hashCode = hashCode * -1521134295 + EqualityComparer<IFinancialAnalysis>.Default.GetHashCode(FinancialAnalysis);
            return hashCode;
        }
    }
}
