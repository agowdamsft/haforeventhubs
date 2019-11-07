using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventhubConsumer
{
    public class EPHManager
    {
        private readonly string geoDrAliasConnectionString;
        private readonly string eventHubName;
        private readonly string storageConnectionString;
        private readonly string[] storageContainers;
        private EventProcessorHost currentEPH;
        private DateTime lastFailover;
        private int currentContainerIdx = 0;


        public EPHManager(string geoDrAliasConnectionString, string eventHubName, string storageConnectionString, string[] storageContainers)
        {
            this.geoDrAliasConnectionString = geoDrAliasConnectionString;
            this.eventHubName = eventHubName;
            this.storageConnectionString = storageConnectionString;
            this.storageContainers = storageContainers;
        }

        public async Task StartAsync(CancellationToken cts = default)
        {
            if (currentEPH != null) throw new InvalidOperationException("already initialized");
            GeoDrWatch.Instance.OnFailover += OnFailover;
            await SetupEphAsync();
        }

        private void OnFailover(object sender, EventArgs e)
        {
            lock (currentEPH)
            {
                if ((DateTime.UtcNow - lastFailover).TotalSeconds > 30)
                {
                    Console.WriteLine("Failover detected. Re-initializing EPH.");
                    lastFailover = DateTime.UtcNow;
                    Task.Run(async () =>
                    {
                        currentContainerIdx = (currentContainerIdx + 1) % storageContainers.Length;
                        await currentEPH.UnregisterEventProcessorAsync();
                        await SetupEphAsync();
                    });
                }
            }
        }

        public async Task StopAsync(CancellationToken cts = default)
        {
            await currentEPH.UnregisterEventProcessorAsync();
        }

        private async Task SetupEphAsync()
        {
            currentEPH = new EventProcessorHost(
                            eventHubName,
                            PartitionReceiver.DefaultConsumerGroupName,
                            geoDrAliasConnectionString,
                            storageConnectionString,
                            storageContainers[currentContainerIdx]);

            var options = new EventProcessorOptions();
            if (storageContainers.Length == 1)
            {
                options.InitialOffsetProvider = (p) => EventPosition.FromEnqueuedTime(DateTime.UtcNow);
            }
            await currentEPH.RegisterEventProcessorFactoryAsync(new GeoDrEventConsumerFactory(storageContainers.Length > 1), options);
        }
    }
}
