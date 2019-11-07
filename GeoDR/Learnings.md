## GEO DR What it Means for the Active Hubs
### Primary and Secondary in a paired event hubs
When you perform a manual failover the primary Event Hubs (master) cannot be used again to point back. It should be deleted and rec-created with the same name (if required) and then added to the same alias.

### Recommendations from PG

Solution 1: Maintain a secondary processor host group
1. Deploy consumers on secondary region that will run with their own dedicated storage account for lease management. This group will stay dormant until the failover day.
2.	Once primary host group starts bleeding invalid-index failures due to failover, shut them down and start secondary host group. They should start reading from offset -1.

Solution 2: Manually switch to an empty lease container.
1.	Shut all hosts down.
2.	Rename existing lease container so you can reuse it when switching back to primary namespace.
3.	Start all hosts and they should create brand new lease container and start reading from the start of the stream on the secondary namespace
4.	As part of the primary switch, rename the original container back to original name and restart the hosts.

Solution 3: Implement a custome lease manager that can be signaled with DR failover state
1.	Monitor invalid offset or manually signal failover state that can be caught by custom lease manager
2.	Lease manager recreates leases to start from beginning of the partitions on the secondary namespace
3.	If you have lost the existing leases from primary namespace, you can still start hosts from the end of the stream before switching back to primary namespace.
