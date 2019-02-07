using Autofac;
using Infrastructure;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using RegistryManager.Controllers;
using RegistryManager.Services;

namespace RegistryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Config>().As<IConfig>();
            builder.RegisterType<Controller>().As<IController>();
            builder.RegisterType<RegistryService>().As<IRegistryService>();
            builder.RegisterType<IsinsCsvFileRepository>().As<IIsinsRepository>();
            builder.RegisterType<MarketDataCsvFileRepository>().As<IMarketDataRepository>();
            builder.RegisterType<RegistryCsvFileRepository>().As<IRegistryRepository>();
            builder.RegisterType<FileSystemFacade>().As<IFileSystemFacade>();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IController>();
                controller.Update();
            }
        }
    }
}
