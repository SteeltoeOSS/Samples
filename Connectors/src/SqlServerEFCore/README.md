# SQL Server Connector Sample App - Entity Framework Core

ASP.NET Core sample app illustrating how to use Entity Framework Core together with [Steeltoe SQL Server Connector](https://docs.steeltoe.io/api/v3/connectors/microsoft-sql-server.html) for connecting to a SQL Server service on CloudFoundry.

## General Pre-requisites

1. Installed .NET Core SDK

## Running Locally

1. Installed SQL Server
1. Created SQL Server database and user with appropriate access level
1. Set [ASPNETCORE_ENVIRONMENT=Development] (<https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments>)
1. Added your connection string to appsettings.development.json under SqlServer:Credentials:ConnectionString

## Running on CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. A SQL Server Instance
1. (Optional) Installed [Cloud Service Broker for VMware Tanzu](https://docs.vmware.com/en/Cloud-Service-Broker-for-VMware-Tanzu/index.html)

## Create Sql Service Instance on CloudFoundry

You must first create an instance of the SQL Server service in an org/space using either a user provided service ('cf cups...' replacing values between pipes as appropriate) OR bind a service using the SQL Server Connector

```bash
> cf target -o myorg -s development
> cf cups mySqlServerService -p '{\"pw\": \"|password|\",\"uid\": \"|user id|\",\"uri\": \"jdbc:sqlserver://|host|:|port|;databaseName=|database name|\"}'
# or
> cf create-service SqlServer sharedVM mySqlServerService
```

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/SqlServerEFCore`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       * `dotnet restore --configfile nuget.config`
       * `dotnet publish -r win-x64 --self-contained`
     - Push the app using the appropriate manifest:
       * `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       * `cf push -f manifest.yml`

> Note: The provided manifest will create an app named `sqlserverefcore-connector` and attempt to bind the app to SQL Server service `mySqlServerService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs sqlserverefcore-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path:  /home/vcap/app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:8080
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://sqlserverefcore-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts two rows into the bound SQL Server database. Those rows are displayed on the home page of the application.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
