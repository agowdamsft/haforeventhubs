using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventhubConsumer
{
    class GeoDrEventConsumerFactory : IEventProcessorFactory
    {
        private readonly bool useCheckpointing;

        public GeoDrEventConsumerFactory(bool useCheckpointing)
        {
            this.useCheckpointing = useCheckpointing;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new GeoDrEventConsumer(useCheckpointing);
        }
    }
}
