# MySql Connector Sample App - EntityFramework Core

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BConnectors_MySqlEFCore%5D?branchName=main)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=18&branchName=main)

ASP.NET Core sample app illustrating how to use Entity Framework Core together with [Steeltoe MySql Connector](https://docs.steeltoe.io/api/v3/connectors/mysql.html) for connecting to a MySql service on CloudFoundry. There is also an additional sample which illustrates how to use a `MySqlConnection` to issue commands to the bound database.

## General Pre-requisites

1. Installed .NET Core SDK

## Running Locally

1. Installed MySQL Server
1. Created MySQL database and user with appropriate access level
1. Set [ASPNETCORE_ENVIRONMENT=Development] (<https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments>)
1. Added your connection string to appsettings.development.json under MySql:Client:ConnectionString

## Running on CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed MySql CloudFoundry service

## Create MySql Service Instance on CloudFoundry

You must first create an instance of the MySql service in an org/space.

1. `cf target -o myorg -s development`
1. `cf create-service p.mysql db-small myMySqlService`

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/MySqlEFCore`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       * `dotnet restore --configfile nuget.config`
       * `dotnet publish -r win-x64`
     - Push the app using the appropriate manifest:
       * `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       * `cf push -f manifest.yml`

> Note: The provided manifest will create an app named `mysqlefcore-connector` and attempt to bind the app to MySql service `myMySqlService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs mysqlefcore-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path:  /home/vcap/app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:8080
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://mysqlefcore-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple rows into the bound MySql database. To display those rows, click on the `MySql Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information.
