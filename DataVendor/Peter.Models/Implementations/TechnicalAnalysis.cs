using Peter.Models.Enums;
using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class TechnicalAnalysis : ITechnicalAnalysis
    {
        public decimal FastSMA { get; set; }
        public decimal SlowSMA { get; set; }
        public TAZ TAZ { get; set; }
    }
}
