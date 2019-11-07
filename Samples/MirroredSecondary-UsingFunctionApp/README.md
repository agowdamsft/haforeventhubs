***Introduction***

This sample describes an alternative to setting up replication from the primary event hub to the secondary utilising an Azure Function App (see other Mirrored Secondary sample for info on setting this up with Confluence MirrorMaker). Using a Functions App with the Event Hub trigger allows us to replicate across messages (non-realtime) at a high throughput as Functions can spawn up mutiple instances to scale to the load of incoming messages. 

***Scenario overview***

Like the MirrorMaker sample, use this template to create the namespaces and to attach a DR configuration to it so that and alias will be created for producers and consumer to connect to (this eliminates the need to change those clients' configuration mid-flight).

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagowdamsft%2Fhaforeventhubs%2Fdev%2FSamples%2FMirroredSecondary%2Ftemplate.json" target="_blank">
    <img src="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.png"/>
</a>

When the resources are deployed look up the outputs from the template