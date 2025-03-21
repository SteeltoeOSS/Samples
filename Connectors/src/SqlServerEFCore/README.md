# SQL Server Connector Sample App - Entity Framework Core

ASP.NET Core sample app illustrating how to use Entity Framework Core together with the [Steeltoe SQL Server Connector](https://docs.steeltoe.io/api/v4/connectors/microsoft-sql-server.html)
for connecting to a Microsoft SQL Server database.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   with [Tanzu Cloud Service Broker for Microsoft Azure](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-microsoft-azure/1-12/csb-azure/reference-azure-mssql-db.html)
   and [Cloud Foundry CLI](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/6-0/tpcf/cf-cli-index.html)

## Running locally

1. Use LocalDB or start a SQL Server [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
   - When using docker, update your connection string in `appsettings.Development.json` accordingly
1. Run the sample
   ```
   dotnet run
   ```

Upon startup, the app inserts a couple of rows into the bound SQL Server database. They are displayed on the home page.

## Running on Tanzu Platform for Cloud Foundry

1. Create a SQL Server service instance in an org/space
   ```
   cf target -o your-org -s your-space
   cf create-service csb-azure-mssql mini sampleSqlServerService
   ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs sqlserver-efcore-connector-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Copy the value of `routes` in the output and open in your browser

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
