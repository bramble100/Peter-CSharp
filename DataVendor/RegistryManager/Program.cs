using RegistryManager.Controllers;
using System;
using System.Configuration;

namespace RegistryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            new Controller().Update();
        }
    }
}
