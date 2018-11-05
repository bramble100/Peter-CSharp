using DataVendor.Controllers.Implementations;
using NLog;
using System;
using System.Configuration;
using System.Linq;

namespace DataVendor
{
    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var reader = new AppSettingsReader();

            try
            {
                if (!args.Any() || Equals(args[0].ToLower(), reader.GetValue("FetchNewMarketData", typeof(string))))
                {
                    new Controller().WebToCsv();
                }
                else if (Equals(args[0].ToLower(), reader.GetValue("UpdateMarketDataWithISINs", typeof(string))))
                {
                    new Controller().AddIsins();
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
