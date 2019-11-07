using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventhubPublisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile("appsettings.local.json", true, true)
               .Build();

            var connectionStringBuilder = new EventHubsConnectionStringBuilder(config["EventHubConnectionString"])
            {
                EntityPath = config["EventHubName"]
            };

            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            CancellationTokenSource cts = new CancellationTokenSource();
            var t = StartAsync(eventHubClient, cts.Token);

            Console.WriteLine("Press ENTER to stop");
            Console.ReadLine();
            cts.Cancel();
            await t;

            await eventHubClient.CloseAsync();
            Console.WriteLine("goodbye");
        }

        private static Task StartAsync(EventHubClient eventHubClient, CancellationToken token)
        {
            return Task.Run(async () => 
            {
                var idx = 0;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        Console.WriteLine($"Sending message: {idx}");
                        await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(idx++.ToString())));
                        await Task.Delay(1000);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                    }
                }
            });
        }
    }
}
