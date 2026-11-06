# CosmosDB Connector Sample App

ASP.NET Core sample app illustrating how to use the [Steeltoe CosmosDB Connector](https://docs.steeltoe.io/api/v4/connectors/cosmosdb.html)
for connecting to a CosmosDB database.

## General pre-requisites

1. Installed .NET 10 SDK

## Running locally

1. Start a CosmosDB [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Run the sample
   ```shell
   dotnet run
   ```

Upon startup, the app inserts a couple of objects into the bound CosmosDB database. They are displayed on the home page.

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
