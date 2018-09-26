using AnalysesManager.Controllers;

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
