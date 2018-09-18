using DataVendor.Controllers;
using System;
using System.Configuration;
using System.Linq;

namespace DataVendor
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new AppSettingsReader();

            if (!args.Any() || string.Equals(args[0].ToLower(), reader.GetValue("FetchNewMarketData", typeof(string))))
            {
                new Controller().WebToCsv();
            }
            else if(string.Equals(args[0].ToLower(), reader.GetValue("UpdateMarketDataWithISINs", typeof(string))))
            {
                new Controller().AddIsins();
            }
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
