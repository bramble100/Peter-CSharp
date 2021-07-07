using Models.Interfaces;

namespace Models.Implementations
{
    internal class FundamentalAnalysis : IFundamentalAnalysis
    {
        public FundamentalAnalysis()
        {
        }

        public decimal PE { get; set; }

        public override bool Equals(object obj) => Equals(obj as FundamentalAnalysis);

        public bool Equals(IFundamentalAnalysis other) => other != null && PE == other.PE;

        public override int GetHashCode() => 380272302 + PE.GetHashCode();

        public override string ToString() => $"PE: {PE}";
    }
}
