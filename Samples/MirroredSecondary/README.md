***Introduction***

This sample describes how to setup a scenario that duplicates events from the primary hub to the secondary. This could be useful in case consumer want to be able to place their cursor back in time on the fallback hub in a DR scenario. By replicating incoming messages directly (not in real-time) into the secondary any client can still read back events that were ingested prior to the failover moment.

***Scenario overview***

Kafka's MirrorMaker is used in the sample to replicate incoming events. Prior to running that a set of Event Hub namespaces have to be present.

Use this template to create the namespaces and to attach a DR configuration to it so that and alias will be created for producers and consumer to connect to (this eliminates the need to change those clients' configuration mid-flight).

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagowdamsft%2Fhaforeventhubs%2Fdev%2FSamples%2FMirroredSecondary%2Ftemplate.json" target="_blank">
    <img src="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.png"/>
</a>

When the resources are deployed lookup the outputs from the template


```
az container create --resource-group $your-resource-group --name kakfa-mirrormaker --image confluentinc/cp-kafka --gitrepo-url https://github.com/agowdamsft/haforeventhubs/blob/dev/Samples/MirroredSecondary --gitrepo-mount-path /mnt/MirrorMaker --command-line "/bin/bash ./mnt/MirrorMaker/mirrormakerconfig.sh " --environment-variables SOURCE_CON_STR="{connection-string-to-source}" DEST_CON_STR="{connection-string-to-source}"
```



![alt text](https://github.com/agowdamsft/haforeventhubs/blob/dev/Samples/MirroredSecondary/Overview.png "Overview")



[Based on @djrosanova 's article](https://github.com/djrosanova/EventHubsMirrorMaker)