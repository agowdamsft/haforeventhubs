using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EventhubConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();

            var secondary = config["StorageContainerSecondary"];
            var primary = config["StorageContainerPrimary"];

            var containers = !String.IsNullOrEmpty(secondary) ? new string[2] { primary, secondary } : new string[1] { primary };

            var ephManager = new EPHManager(config["EventHubConnectionString"], config["EventHubName"], config["StorageConnectionString"], containers);
            await ephManager.StartAsync();
            
            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            await ephManager.StopAsync();
        }
    }
}
