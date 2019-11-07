# Creating a mirrored secondary Event Hub using a Function #
## Introduction ##

This sample describes an alternative to setting up replication from the primary event hub to the secondary utilising an Azure Function App (see other Mirrored Secondary sample for info on setting this up with Confluence MirrorMaker). Using a Functions App with the Event Hub trigger allows us to replicate across messages (non-realtime) at a high throughput as Functions can spawn up multiple instances to scale to the load of incoming messages. 

>> Note: this is purely an experiment to see if Functions can handle enough throughput to serve as a viable replication option (results of tests are shared later in this readme). Use at your own risk!

## Deploy the resources ##

Like the MirrorMaker sample, use this template to create the namespaces and to attach a DR configuration to it so that and alias will be created for producers and consumer to connect to (this eliminates the need to change those clients' configuration mid-flight).

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagowdamsft%2Fhaforeventhubs%2Fdev%2FSamples%2FMirroredSecondary%2Ftemplate.json" target="_blank">
    <img src="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.png"/>
</a>

When the resources are deployed look up the outputs from the template to get the primary and secondary event hub connection strings and note these down - you'll need them for the Function's app settings. Now head to the Azure portal and create an Event Hub entity in each of the namespaces. You can name them whatever you like (just jot down what you call them as you'll need it afterwards) and provision as many partitions as you'd like - more partitions will allow more throughput. 

<img src="https://raw.githubusercontent.com/agowdamsft/haforeventhubs/master/Samples/MirroredSecondary-UsingFunctionApp/eventhub_entity_creation.jpg"/>

Next open up the Functions App in your favourite IDE and edit the EventHubReplicator.cs file, changing YOUR_PRIMARY_EH_NAME & YOUR_SECONDARY_EH_NAME to the names of the Event Hub entities you just created. 

<img src="https://raw.githubusercontent.com/agowdamsft/haforeventhubs/master/Samples/MirroredSecondary-UsingFunctionApp/function_eventhub_triggername.jpg"/>

Then save your changes and deploy the Functions App to Azure in the same region as your secondary event hub. You could do this in a variety of ways - I used Visual Studio Code with the Functions extension. Create the app to run .NET Core with a Consumption plan (you could alternatively try fixed compute to compare results - i.e. a Premium plan).

Once this is deployed, head to the Function App's app settings (click on the deployed Function App in the Azure portal > Platform Features > Function App Settings > Manage Application Settings) and add your primary and secondary Event Hub connection strings (which you jotted down earlier):

<img src="https://raw.githubusercontent.com/agowdamsft/haforeventhubs/master/Samples/MirroredSecondary-UsingFunctionApp/function_app_settings1.jpg"/>

Now your infrastructure is set up. To test your Function replication it's time to start throwing some load at the primary event hub. You can do this in several ways; I used <a href="https://github.com/Azure-Samples/durable-functions-producer-consumer" target="_blank">this producer simulator available on GitHub.</a>
