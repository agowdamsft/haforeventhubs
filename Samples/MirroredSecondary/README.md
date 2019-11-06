***Introduction***

This sample describes how to setup a scenario that duplicates events from the primary hub to the secondary. This could be useful in case consumer want to be able to place their cursor back in time on the fallback hub in a DR scenario. By replicating incoming messages directly (not in real-time) into the secondary any client can still read back events that were ingested prior to the failover moment.

***Scenario overview***

Kafka's mirror makes is used in the sample to replicate incoming events. 

```
az container create --resource-group $your-resource-group --name kakfa-mirrormaker --image confluentinc/cp-kafka --gitrepo-url https://github.com/agowdamsft/haforeventhubs/blob/dev/Samples/MirroredSecondary --gitrepo-mount-path /mnt/MirrorMaker --command-line "/bin/bash ./mnt/MirrorMaker/mirrormakerconfig.sh " --environment-variables SOURCE_CON_STR="{connection-string-to-source}" DEST_CON_STR="{connection-string-to-source}"
```

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fazure-quickstart-templates%2Fmaster%2F201-eventhub-create-namespace-geo-recoveryconfiguration%2Fazuredeploy.json" target="_blank">
    <img src="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.png"/>
</a>

![alt text](https://github.com/agowdamsft/haforeventhubs/blob/dev/Samples/MirroredSecondary/Overview.png "Overview")



[Based on @djrosanova 's article](https://github.com/djrosanova/EventHubsMirrorMaker)