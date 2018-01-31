# SQL Server Connector Sample App - SQLConnection

ASP.NET 4.x sample app illustrating how to use [Steeltoe Microsoft SQL Server Connector](https://github.com/SteeltoeOSS/Connectors) for connecting to a Microsoft SQL Server service on CloudFoundry using Entity Framework.

This sample makes use of StructureMap for IOC services.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
1. Installed Windows support
1. A SQL Server Instance (and optionally the [Microsoft SQL Server Broker for CloudFoundry](https://github.com/cf-platform-eng/mssql-server-broker) installed)

## Create Microsoft SQL Service Instance on CloudFoundry

You must first create an instance of the Microsoft SQL Server service in a org/space using either a user provided service ('cf cups...' replacing values between pipes as appropriate) OR bind a service using the Microsoft SQL Server Connector

```bash
> cf target -o myorg -s development

> cf cups mySqlServerService -p '{\"pw\": \"|password|\",\"uid\": \"|user id|\",\"uri\": \"jdbc:sqlserver://|host|:|port|;databaseName=|database name|\"}'<br>
or
> cf create-service SqlServer sharedVM mySqlServerService
```

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select MsSql4 project in Solution Explorer.
1. Right-click and select Publish
1. Publish the App to a folder. (e.g. c:\publish)
1. cd publish_folder (e.g. cd c:\publish)
1. cf push

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs mssql4-connector`

You should see something like this during startup:

```bash
2016-12-05T11:26:33.12-0700 [STG/0]      OUT Successfully destroyed container
2016-12-05T11:26:34.60-0700 [CELL/0]     OUT Successfully created container
2016-12-05T11:26:40.49-0700 [APP/0]      OUT Running ..\tmp\lifecycle\WebAppServer.exe
2016-12-05T11:26:40.55-0700 [APP/0]      OUT PORT == 52222
2016-12-05T11:26:40.56-0700 [APP/0]      OUT 2016-12-05 18:26:40Z|INFO|Port:52222
2016-12-05T11:26:40.56-0700 [APP/0]      OUT 2016-12-05 18:26:40Z|INFO|Webroot:C:\containerizer\D01F08F4D6E6E541C6\user\app
2016-12-05T11:26:40.63-0700 [APP/0]      OUT 2016-12-05 18:26:40Z|INFO|Starting web server instance...
2016-12-05T11:26:40.74-0700 [APP/0]      OUT Server Started.... press CTRL + C to stop
2016-12-05T11:26:46.50-0700 [APP/0]      OUT Initializing data
2016-12-05T11:26:48.16-0700 [APP/0]      OUT Initializing data complete!
```

At this point the app is up and running.  Upon startup the app inserts a couple rows into the bound Microsoft SQL Server database. To display those rows browse to the app and you should see the row data displayed.
