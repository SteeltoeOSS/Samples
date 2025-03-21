# CosmosDB Connector Sample App

ASP.NET Core sample app illustrating how to use the [Steeltoe CosmosDB Connector](https://docs.steeltoe.io/api/v4/connectors/cosmosdb.html)
for connecting to a CosmosDB database.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   with [Tanzu Cloud Service Broker for Microsoft Azure](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-microsoft-azure/1-10/csb-azure/reference-azure-cosmosdb-sql.html)
   and [Cloud Foundry CLI](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/6-0/tpcf/cf-cli-index.html)

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

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
