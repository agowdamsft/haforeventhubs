using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventhubConsumer
{
    public class EPHManager
    {
        private readonly string geoDrAliasConnectionString;
        private readonly string storageConnectionString;
        private readonly string[] storageContainers;

        public EPHManager(string geoDrAliasConnectionString, string storageConnectionString, string[] storageContainers)
        {
            this.geoDrAliasConnectionString = geoDrAliasConnectionString;
            this.storageConnectionString = storageConnectionString;
            this.storageContainers = storageContainers;
        }

        public Task StartAsync(CancellationToken cts = default)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cts = default)
        {
            return Task.CompletedTask;
        }
    }
}
