using Peter.Models.Interfaces;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    public class Analysis : IAnalysis
    {
        public string Name { get; set; }
        public decimal ClosingPrice { get; set; }
        public int QtyInBuyingPacket { get; set; }
        public ITechnicalAnalysis TechnicalAnalysis { get; set; }
        public IFinancialAnalysis FinancialAnalysis { get; set; }

        public override bool Equals(object obj) => Equals(obj as Analysis);

        public bool Equals(IAnalysis other)
        {
            return other != null &&
                   Name == other.Name &&
                   ClosingPrice == other.ClosingPrice &&
                   QtyInBuyingPacket == other.QtyInBuyingPacket &&
                   EqualityComparer<ITechnicalAnalysis>.Default.Equals(TechnicalAnalysis, other.TechnicalAnalysis) &&
                   EqualityComparer<IFinancialAnalysis>.Default.Equals(FinancialAnalysis, other.FinancialAnalysis);
        }

        public override int GetHashCode()
        {
            var hashCode = 1319828343;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + ClosingPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + QtyInBuyingPacket.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ITechnicalAnalysis>.Default.GetHashCode(TechnicalAnalysis);
            hashCode = hashCode * -1521134295 + EqualityComparer<IFinancialAnalysis>.Default.GetHashCode(FinancialAnalysis);
            return hashCode;
        }

        public override string ToString() => $"{Name} Closing: {ClosingPrice} {QtyInBuyingPacket}";
    }
}
