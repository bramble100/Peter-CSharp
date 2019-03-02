using Autofac;
using Infrastructure;
using NLog;
using Peter.Repositories.Implementations;
using Peter.Repositories.Interfaces;
using System;
using System.Linq;

namespace CLI
{
    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string helpText = "Peter - Stock Exchange Screener - v1.0.4.\n" + 
            "Usage:\n" +
            "CLI analyses: create analyses based on the available data";

        static void Main(string[] args)
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<ConfigReader>().As<IConfigReader>();
                builder.RegisterType<FileSystemFacade>().As<IFileSystemFacade>();

                builder.RegisterType<Services.Analyses.Service>().As<Services.Analyses.IService>();
                builder.RegisterType<Services.Registry.Service>().As<Services.Registry.IService>();
                builder.RegisterType<Services.DataVendor.WebService>().As<Services.DataVendor.IWebService>();
                builder.RegisterType<Services.DataVendor.IsinAdderService>().As<Services.DataVendor.IIsinAdderService>();

                builder.RegisterType<AnalysesCsvFileRepository>().As<IAnalysesRepository>();
                builder.RegisterType<IsinsCsvFileRepository>().As<IIsinsRepository>();
                builder.RegisterType<MarketDataCsvFileRepository>().As<IMarketDataRepository>();
                builder.RegisterType<RegistryCsvFileRepository>().As<IRegistryRepository>();

                var container = builder.Build();
                using (var scope = container.BeginLifetimeScope())
                {
                    var _configReader = scope.Resolve<IConfigReader>();
                    Console.WriteLine(helpText);

                    if (!args.Any())
                    {
                        _logger.Fatal("No parameter given.");
                    }
                    else if (args.Count() > 1)
                    {
                        _logger.Fatal("Too many parameters given.");
                        _logger.Info(string.Join(" ", args));
                    }
                    else
                    {
                        string command = args.Single();

                        if (string.Equals(command, _configReader.Settings.FetchNewMarketData))
                        {
                            _logger.Info(_configReader.Settings.FetchNewMarketData);
                            var service = scope.Resolve<Services.DataVendor.IWebService>();

                            var marketData = service.GetDownloadedDataFromWeb();
                            service.Update(marketData);
                        }
                        else if (string.Equals(command, _configReader.Settings.UpdateMarketDataWithISINs))
                        {
                            _logger.Info(_configReader.Settings.UpdateMarketDataWithISINs);

                            scope
                                .Resolve<Services.DataVendor.IIsinAdderService>()
                                .AddIsinsToMarketData();
                        }
                        else if (string.Equals(command, "registry"))
                        {
                            _logger.Info("registry");

                            scope
                                .Resolve<Services.Registry.IService>()
                                .Update();
                        }
                        else if (string.Equals(command, "analyse"))
                        {
                            _logger.Info("analyse");

                            scope
                                .Resolve<Services.Analyses.IService>()
                                .GenerateAnalyses();
                        }
                        else
                        {
                            _logger.Fatal("Unknown parameter given.");
                            _logger.Info(command);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            LogManager.Shutdown();
        }
    }
}
