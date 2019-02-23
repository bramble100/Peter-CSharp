using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    internal class FinancialAnalysis : IFinancialAnalysis
    {
        public FinancialAnalysis()
        {
        }

        public decimal PE { get; set; }

        public override bool Equals(object obj) => Equals(obj as FinancialAnalysis);

        public bool Equals(IFinancialAnalysis other) => other != null && PE == other.PE;

        public override int GetHashCode() => 380272302 + PE.GetHashCode();

        public override string ToString() => $"PE: {PE}";
    }
}
