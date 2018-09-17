using DataVendor.Controllers;
using System;
using System.Linq;

namespace DataVendor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any() || string.Equals(args[0].ToLower(), "fetch"))
            {
                new Controller().WebToCsv();
            }
            else if(string.Equals(args[0].ToLower(), "addisins"))
            {
                new Controller().AddIsins();
            }
            Console.ReadKey();
        }
    }
}
