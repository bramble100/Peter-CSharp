using AnalysesManager.Controllers.Implementations;

namespace AnalysesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            new Controller().GenerateAnalyses();
        }
    }
}
