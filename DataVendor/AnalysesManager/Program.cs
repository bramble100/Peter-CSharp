using AnalysesManager.Controllers.Implementations;
using AnalysesManager.Services.Implementations;
using AnalysesManager.Services.Interfaces;
using Autofac;

namespace AnalysesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new Service()).As<IService>();
            var container = builder.Build();

            new Controller().GenerateAnalyses();
        }
    }
}
