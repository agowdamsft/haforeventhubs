using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace EventhubConsumer
{
    internal interface IPartitionErrorHandler
    {
        Task<bool> ProcessErrorAsync(PartitionContext context, Exception error);
    }
}
