using System;
using System.Threading.Tasks;

namespace EventhubConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");
            var ephManager = new EPHManager("", "", "", null);
            await ephManager.StartAsync();
            
            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            await ephManager.StopAsync();
        }
    }
}
