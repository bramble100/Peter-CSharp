using RegistryManager.Controllers;
using System;
using System.Configuration;

namespace RegistryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new AppSettingsReader();

            new Controller().Update();

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
