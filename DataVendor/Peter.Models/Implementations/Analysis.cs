using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class Analysis : IAnalysis
    {
        public string Name { get; set; }
        public decimal ClosingPrice { get; set; }
        public int QtyInBuyingPacket { get; set; }
        public ITechnicalAnalysis TechnicalAnalysis { get; set; }
        public IFinancialAnalysis FinancialAnalysis { get; set; }
    }
}
