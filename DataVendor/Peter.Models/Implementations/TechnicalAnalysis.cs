using Peter.Models.Enums;
using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    internal class TechnicalAnalysis : ITechnicalAnalysis
    {
        public decimal FastSMA { get; set; }
        public decimal SlowSMA { get; set; }
        public TAZ TAZ { get; set; }
        public Trend Trend { get; set; }

        public override bool Equals(object obj) => Equals(obj as TechnicalAnalysis);

        public bool Equals(ITechnicalAnalysis other) => other != null &&
                   FastSMA == other.FastSMA &&
                   SlowSMA == other.SlowSMA &&
                   TAZ == other.TAZ &&
                   Trend == other.Trend;

        public override int GetHashCode()
        {
            var hashCode = 1276244321;
            hashCode = hashCode * -1521134295 + FastSMA.GetHashCode();
            hashCode = hashCode * -1521134295 + SlowSMA.GetHashCode();
            hashCode = hashCode * -1521134295 + TAZ.GetHashCode();
            hashCode = hashCode * -1521134295 + Trend.GetHashCode();
            return hashCode;
        }

        public override string ToString() => 
            $"FastSMA: {FastSMA} SlowSMA: {SlowSMA} TAZ: {TAZ} Trend: {Trend}";
    }
}