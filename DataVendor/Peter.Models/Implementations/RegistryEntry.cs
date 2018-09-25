using Peter.Models.Enums;
using Peter.Models.Interfaces;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    public class RegistryEntry : IRegistryEntry
    {
        public string Name { get; set; }
        public string OwnInvestorLink { get; set; }
        public string StockExchangeLink { get; set; }
        public Position Position { get; set; }
        public IFinancialReport FinancialReport { get; set; }
        public IFinancialAnalysis FinancialAnalysis { get; set; }

        public RegistryEntry()
        {
        }

        public RegistryEntry(string name)
        {
            Name = name;
        }

        public bool Equals(IRegistryEntry other)
        {
            return other != null &&
                   Name == other.Name &&
                   EqualityComparer<string>.Default.Equals(OwnInvestorLink, other.OwnInvestorLink) &&
                   EqualityComparer<string>.Default.Equals(StockExchangeLink, other.StockExchangeLink) &&
                   Position == other.Position &&
                   EqualityComparer<IFinancialReport>.Default.Equals(FinancialReport, other.FinancialReport) &&
                   EqualityComparer<IFinancialAnalysis>.Default.Equals(FinancialAnalysis, other.FinancialAnalysis);
        }

        public override bool Equals(object obj) => Equals(obj as IRegistryEntry);

        public override int GetHashCode()
        {
            var hashCode = 741974547;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OwnInvestorLink);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StockExchangeLink);
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IFinancialReport>.Default.GetHashCode(FinancialReport);
            hashCode = hashCode * -1521134295 + EqualityComparer<IFinancialAnalysis>.Default.GetHashCode(FinancialAnalysis);
            return hashCode;
        }
    }
}
