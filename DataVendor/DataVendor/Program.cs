using Autofac;
using DataVendor.Controllers.Implementations;
using DataVendor.Controllers.Interfaces;
using DataVendor.Services;
using Infrastructure;
using NLog;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System;
using System.Linq;

namespace DataVendor
{
    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _logger.Info("*** DataVendor ***");

            var builder = new ContainerBuilder();
            builder.RegisterType<Config>().As<IConfig>();
            builder.RegisterType<Controller>().As<IController>();
            builder.RegisterType<IsinAdderService>().As<IIsinAdderService>();
            builder.RegisterType<WebService>().As<IWebService>();
            builder.RegisterType<IsinsCsvFileRepository>().As<IIsinsRepository>();
            builder.RegisterType<MarketDataCsvFileRepository>().As<IMarketDataRepository>();
            builder.RegisterType<FileSystemFacade>().As<IFileSystemFacade>();

            var container = builder.Build();
            using(var scope = container.BeginLifetimeScope())
            {
                var config = scope.Resolve<IConfig>();
                var controller = scope.Resolve<IController>();

                try
                {
                    if (!args.Any() || Equals(args[0].ToLower(), config.GetValue<string>("FetchNewMarketData")))
                    {
                        controller.WebToCsv();
                    }
                    else if (Equals(args[0].ToLower(), config.GetValue<string>("UpdateMarketDataWithISINs")))
                    {
                        controller.AddIsins();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            _logger.Info("*** *** ***");

            LogManager.Shutdown();
        }
    }
}
