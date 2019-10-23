using Autofac;
using Infrastructure;
using NLog;
using Repositories.Implementations;
using Repositories.Interfaces;
using System;
using System.Linq;

namespace CLI
{
    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string helpText = "Peter - Stock Exchange Screener - v1.0.7.\n" +
            "Usage:\n" +
            "CLI analyses: create analyses based on the available data";

        static void Main(string[] args)
        {
            try
            {
                using (var scope = NewDIContainer().BeginLifetimeScope())
                {
                    var _configReader = scope.Resolve<IConfigReader>();
                    Console.WriteLine(helpText);

                    if (!args.Any())
                    {
                        _logger.Info("Test mode (without external dependencies).");

                        TestAnalyser(scope);
                        _logger.Info("Analyser OK.");
                        TestDatavendor(scope);
                        _logger.Info("Datavendor OK.");
                        TestRegistry(scope);
                        _logger.Info("Registry OK .");

                        Console.WriteLine("Press any key ...");
                        Console.ReadKey();
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
                            var marketData = service.GetDownloadedDataFromWeb().GetAwaiter().GetResult();
                            service.UpdateMarketData(marketData);
                        }
                        else if (string.Equals(command, _configReader.Settings.UpdateMarketDataWithISINs))
                        {
                            _logger.Info(_configReader.Settings.UpdateMarketDataWithISINs);
                            var service = scope.Resolve<Services.DataVendor.IIsinAdderService>();
                            service.AddIsinsToMarketData();
                        }
                        else if (string.Equals(command, "registry"))
                        {
                            _logger.Info("registry");
                            var service = scope.Resolve<Services.Registry.IService>();
                            service.Update();
                        }
                        else if (string.Equals(command, "analyse"))
                        {
                            _logger.Info("analyse");
                            var service = scope.Resolve<Services.Analyses.IService>();
                            service.NewAnalyses();
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
                _logger.Error(ex.Message);
                _logger.Debug(ex);
            }

            LogManager.Shutdown();
        }

        private static void TestAnalyser(ILifetimeScope scope) => scope.Resolve<Services.Analyses.IService>();

        private static void TestDatavendor(ILifetimeScope scope) => scope.Resolve<Services.DataVendor.IWebService>();

        private static void TestRegistry(ILifetimeScope scope) => scope.Resolve<Services.Registry.IService>();

        private static IContainer NewDIContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ConfigReader>().As<IConfigReader>();
            builder.RegisterType<EnvironmentVariableReader>().As<IEnvironmentVariableReader>();
            builder.RegisterType<FileSystemFacade>().As<IFileSystemFacade>();
            builder.RegisterType<HttpFacade>().As<IHttpFacade>();

            builder.RegisterType<Services.Analyses.Service>().As<Services.Analyses.IService>();
            builder.RegisterType<Services.Registry.Service>().As<Services.Registry.IService>();
            builder.RegisterType<Services.DataVendor.WebService>().As<Services.DataVendor.IWebService>();
            builder.RegisterType<Services.DataVendor.IsinAdderService>().As<Services.DataVendor.IIsinAdderService>();

            builder.RegisterType<Services.Analyses.FundamentalAnalyser>().As<Services.Analyses.IFundamentalAnalyser>();
            builder.RegisterType<Services.Analyses.TechnicalAnalyser>().As<Services.Analyses.ITechnicalAnalyser>();

            builder.RegisterType<AnalysesCsvFileRepository>().As<IAnalysesRepository>();
            builder.RegisterType<IsinsCsvFileRepository>().As<IIsinsRepository>();
            builder.RegisterType<MarketDataCsvFileRepository>().As<IMarketDataRepository>();
            builder.RegisterType<RegistryCsvFileRepository>().As<IRegistryRepository>();

            var container = builder.Build();
            return container;
        }
    }
}
