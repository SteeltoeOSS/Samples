# CosmosDB Connector Sample App

ASP.NET Core sample app illustrating how to use the [Steeltoe CosmosDB Connector](https://docs.steeltoe.io/api/v3/connectors/cosmosdb.html)
for connecting to a CosmosDB database.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   (optionally with [Windows support](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/concepts-overview.html))
   with [VMware Tanzu Cloud Service Broker](https://docs.vmware.com/en/Cloud-Service-Broker-for-VMware-Tanzu/index.html)
   and [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)

## Running locally

1. Start the [Azure CosmosDB Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator)
1. Update your local primary key in `appsettings.development.json` at `Steeltoe:Client:CosmosDb:Default:ConnectionString`
1. Run the sample
   ```
   dotnet run
   ```

Upon startup, the app inserts a couple of objects into the bound CosmosDB database. They are displayed on the home page.

## Running on Tanzu Platform for Cloud Foundry

1. Create a CosmosDB service instance in an org/space
   ```
   cf target -o your-org -s your-space
   cf create-service csb-azure-cosmosdb-sql small sampleCosmosDbService
   ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs cosmosdb-connector-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Copy the value of `routes` in the output and open in your browser

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
