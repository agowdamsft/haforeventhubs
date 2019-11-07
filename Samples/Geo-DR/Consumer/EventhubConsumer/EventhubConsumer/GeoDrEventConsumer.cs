using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventhubConsumer
{
    internal class GeoDrEventConsumer : IEventProcessor
    {
        private bool failoverStarted;
        private readonly bool useCheckpointing;

        public GeoDrEventConsumer(bool useCheckpointing)
        {
            this.useCheckpointing = useCheckpointing;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            if (GeoDrWatch.IsGeoDRException(error))
            {
                failoverStarted = true;
                GeoDrWatch.Instance.InitiateFailover(this);
            }
            else
            {
                Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            }
            return Task.CompletedTask;
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            if (failoverStarted)
            {
                return;
            }

            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                Console.WriteLine($"{DateTime.Now}: Message received. Partition: '{context.PartitionId}', Data: '{data}'");
            }

            if (useCheckpointing)
            {
                await context.CheckpointAsync();
            }
        }
    }
}
