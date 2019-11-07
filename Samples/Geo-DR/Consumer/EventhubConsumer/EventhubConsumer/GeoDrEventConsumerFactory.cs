using Microsoft.Azure.EventHubs.Processor;

namespace EventhubConsumer
{
    class GeoDrEventConsumerFactory : IEventProcessorFactory
    {
        private readonly bool useCheckpointing;
        private readonly IPartitionErrorHandler errorHandler;

        public GeoDrEventConsumerFactory(bool useCheckpointing, IPartitionErrorHandler errorHandler)
        {
            this.useCheckpointing = useCheckpointing;
            this.errorHandler = errorHandler;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new GeoDrEventConsumer(this.useCheckpointing, this.errorHandler);
        }
    }
}
