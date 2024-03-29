﻿using Microsoft.Azure.EventHubs;
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
        private readonly IPartitionErrorHandler errorHandler;

        public GeoDrEventConsumer(bool useCheckpointing, IPartitionErrorHandler errorHandler)
        {
            this.useCheckpointing = useCheckpointing;
            this.errorHandler = errorHandler;
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

        public async Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            if (this.failoverStarted) return;

            bool canContinue = await this.errorHandler?.ProcessErrorAsync(context, error);
            if (!canContinue)
            {
                this.failoverStarted = true;
            }
            else
            {
                Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            }
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
