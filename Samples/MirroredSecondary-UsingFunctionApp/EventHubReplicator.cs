using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventHubsHADR.ReplicatorFunction
{
    public static class EventHubReplicator
    {
        [FunctionName("EventHubReplicator")]
        public static async Task Run(
            [EventHubTrigger("YOUR_PRIMARY_EH_NAME", Connection = "eventHubPrimaryConnString")] EventData[] events,
            [EventHub("YOUR_SECONDARY_EH_NAME", Connection = "eventHubSecondaryConnString")]IAsyncCollector<EventData> outputEvents,
            ILogger log)
        {
            foreach (EventData eventData in events)
            {
                // add message to forward to secondary
                await outputEvents.AddAsync(eventData);
            }
        }
    }
}
