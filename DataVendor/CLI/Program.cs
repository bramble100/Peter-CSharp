using Autofac;
using Infrastructure;
using NLog;
using System;
using System.Linq;

namespace CLI
{
    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string helpText = "Peter - Stock Exchange Screener - v1.0.3.\n" + 
            "Usage:\n" +
            "CLI analyses: create analyses based on the available data";

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConfigReader>().As<IConfigReader>();
            builder.RegisterType<FileSystemFacade>().As<IFileSystemFacade>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var _configReader = scope.Resolve<IConfigReader>();
                Console.WriteLine(helpText);

                if (!args.Any())
                {
                    Console.WriteLine("No parameter given.");
                }
                else if (args.Count() > 1)
                {
                    Console.WriteLine("Too many parameters given.");
                    Console.WriteLine(string.Join(" ", args));
                }
                else
                {
                    string command = args.Single();

                    if (string.Equals(command, _configReader.Settings.FetchNewMarketData))
                    {
                        Console.WriteLine(_configReader.Settings.FetchNewMarketData);
                    }
                    else if (string.Equals(command, _configReader.Settings.UpdateMarketDataWithISINs))
                    {
                        Console.WriteLine(_configReader.Settings.UpdateMarketDataWithISINs);
                    }
                    else if (string.Equals(command, "registry"))
                    {
                        Console.WriteLine("registry");
                    }
                    else if (string.Equals(command, "analyse"))
                    {
                        Console.WriteLine("analyse");
                    }
                    else
                    {
                        Console.WriteLine("Unknown parameter given.");
                        Console.WriteLine(command);
                    }
                }
            }
            LogManager.Shutdown();
            Console.ReadKey();
        }
    }
}
