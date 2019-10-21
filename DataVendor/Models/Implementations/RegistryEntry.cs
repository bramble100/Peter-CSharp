using Models.Enums;
using Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Models.Implementations
{
    internal class RegistryEntry : IRegistryEntry, IEquatable<RegistryEntry>
    {
        public string Isin { get; set; }
        public string Name { get; set; }
        public Uri OwnInvestorLink { get; set; }
        public Uri StockExchangeLink { get; set; }
        public IFinancialReport FinancialReport { get; set; }
        public IFundamentalAnalysis FundamentalAnalysis { get; set; }

        public RegistryEntry()
        {
        }

        public RegistryEntry(string name, RegistryEntry registryEntry) : this()
        {
            Name = name;
        }

        public bool Equals(IRegistryEntry other) => other != null && Isin == other.Isin;

        public override bool Equals(object obj) => Equals(obj as IRegistryEntry);

        public override string ToString() => $"{Name} ({Isin})";

        public bool Equals(RegistryEntry other) => Equals(other as IRegistryEntry);

        public override int GetHashCode() => 996337662 + EqualityComparer<string>.Default.GetHashCode(Isin);
    }
}
