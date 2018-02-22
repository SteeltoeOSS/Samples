# SQL Server Connector Sample App

This ASP.NET 4.6.1 sample app uses the [Steeltoe Microsoft SQL Server Connector](https://steeltoe.io/docs/steeltoe-connectors/#3-0-microsoft-sql-server) to connect to a Microsoft SQL Server service when running on CloudFoundry.

This sample uses Entity Framework to work with the database and StructureMap for IoC services.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
1. Installed Windows support
1. Installed SQL Server Instance
1. (Optional) [Microsoft SQL Server Broker for CloudFoundry](https://github.com/cf-platform-eng/mssql-server-broker)

## Create Microsoft SQL Service Instance on CloudFoundry

You must first create an instance of the Microsoft SQL Server service in a org/space using either a user provided service ('cf cups...' replacing values between pipes as appropriate) OR bind a service using the Microsoft SQL Server Connector

```bash
> cf target -o myorg -s development
> cf cups mySqlServerService -p '{\"pw\": \"|password|\",\"uid\": \"|user id|\",\"uri\": \"jdbc:sqlserver://|host|:|port|;databaseName=|database name|\"}'<br>
# or
> cf create-service SqlServer sharedVM mySqlServerService
```

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select the MsSql4 project in Solution Explorer.
1. Right-click and select Publish
1. Use `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cd samples\Connectors\src\AspDotNet4\MsSql4
> cf push -p bin\Debug\net461\win10-x64\publish
```

> Note: The provided manifest(s) will create an app named `mssql-connector-4x` and attempt to bind the app to Microsoft SQL Server service `mySqlServerService`.

## What to expect - CloudFoundry

Use the Cloud Foundry CLI to see the logs as you startup and use the app, with the command `cf logs mssql4-connector`

This sample will be available at <http://mssql-connector-4x.[your-cf-apps-domain]/>.

Upon startup, the app inserts two rows into the bound Microsoft SQL Server database. To display those rows, browse to the app and the row data should be on the home page.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-connectors) for guided tour of the samples and more detailed Connector information