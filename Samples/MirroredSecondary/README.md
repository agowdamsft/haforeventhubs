***Introduction***

This sample describes how to setup a scenario that duplicates events from the primary hub to the secondary. This could be useful in case consumer want to be able to place their cursor back in time on the fallback hub in a DR scenario. By replicating incoming messages directly (not in real-time) into the secondary any client can still read back events that were ingested prior to the failover moment.

***Scenario overview***

Kafka's MirrorMaker is used in this sample to replicate incoming events. 

You can Use the template below to create the namespaces and to attach a DR configuration to it so that and alias will be created for producers and consumer to connect to (this eliminates the need to change those clients' configuration mid-flight).

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagowdamsft%2Fhaforeventhubs%2Fdev%2FSamples%2FMirroredSecondary%2Ftemplate.json" target="_blank">
    <img src="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.png"/>
</a>

Copy the output values of this template into the connectionstring placeholders below to spin up a container for a load generator and an instance of the MirrorMaker tool.


```
az container create --resource-group $rgname --name mirrormaker --image confluentinc/cp-kafka --gitrepo-url https://github.com/djrosanova/EventHubsMirrorMaker --gitrepo-mount-path /mnt/EventHubsMirrorMaker --command-line "/bin/bash ./mnt/EventHubsMirrorMaker/ehmirror/mirrorstart.sh " --environment-variables SOURCE_CON_STR="[YOUR-SOURCE-EVENTHUB-CONNECTIONSTRING]" DEST_CON_STR="[YOUR-DESTINATION-EVENTHUB-CONNECTIONSTRING]"
```

```
az container create --resource-group mmrg24173 --name loadgenerator --image confluentinc/cp-kafka --gitrepo-url https://github.com/agowdamsft/haforeventhubs --gitrepo-mount-path /mnt/MirrorMaker --environment-variables SOURCE_CON_STR="[YOUR-SOURCE-EVENTHUB-CONNECTIONSTRING]" --command-line "/bin/bash ./mnt/MirrorMaker/Samples/MirroredSecondary/loadgenerator.sh -d $SOURCE_CON_STR" --restart-policy OnFailure
```

Note that the message replication includes event's Properties (the key value list that can be attached to Event Hub messages) but the values currently are base64 encoded (the source eventhub contains these values as they were submitted to the source Event Hub)

![alt text](https://github.com/agowdamsft/haforeventhubs/blob/dev/Samples/MirroredSecondary/Overview.png "Overview")

[Based on @djrosanova 's article](https://github.com/djrosanova/EventHubsMirrorMaker)