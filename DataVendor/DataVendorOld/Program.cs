using DataVendor.Controllers;
using System;

namespace DataVendor
{
    class Program
    {
        static void Main(string[] args)
        {
            new Controller().WebToCsv();
            Console.ReadKey();
        }
    }
}
