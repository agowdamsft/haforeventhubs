***Introduction***

This sample describes how to setup a scenario that duplicates events from the primary hub to the secondary. This could be useful in case consumer want to be able to place their cursor back in time on the fallback hub in a DR scenario. By replicating incoming messages directly (not in real-time) into the secondary any client can still read back events that were ingested prior to the failover moment.

***Scenario overview***

Kafka's mirror makes is used in the sample to replicate incoming events.

```
az container create --resource-group $your-resource-group --name kakfa-mirrormaker --image confluentinc/cp-kafka --gitrepo-url https://github.com/djrosanova/EventHubsMirrorMaker --gitrepo-mount-path /mnt/EventHubsMirrorMaker --command-line "/bin/bash ./mnt/EventHubsMirrorMaker/ehmirror/mirrorstart.sh " --environment-variables SOURCE_CON_STR="$SOURCE_CON_STR" DEST_CON_STR="$DEST_CON_STR"
```


![alt text](overview.png "Overview")

