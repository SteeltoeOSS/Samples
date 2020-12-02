# MySql Connector Sample App - MySqlConnection

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BConnectors_MySql%5D?branchName=2.x)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=17&branchName=master)

ASP.NET Core sample app illustrating how to use [Steeltoe MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) for connecting to a MySql service on CloudFoundry. This specific sample illustrates how to use a `MySqlConnection` to issue commands to the bound database. There is also an additional samples which illustrate how to use EF6 and EFCore.

## General Pre-requisites

1. Installed .NET Core SDK

## Running Locally

1. Installed MySQL Server
1. Created MySQL database and user with appropriate access level
1. Set [ASPNETCORE_ENVIRONMENT=Development] (<https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments>)
1. Added your connection string to appsettings.development.json under mysql:client:ConnectionString

## Running on CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed MySql CloudFoundry service

## Create MySql Service Instance on CloudFoundry

You must first create an instance of the MySql service in a org/space.

1. `cf target -o myorg -s development`
1. `cf create-service p.mysql db-small myMySqlService`

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/MySql`
1. `dotnet restore --configfile nuget.config`
1. Publish app to a local directory, specifying the framework and runtime (select ONE of these commands):
   * `dotnet publish -f netcoreapp3.1 -r linux-x64`
1. Push the app using the appropriate manifest (select ONE of these commands):
   * `cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish`

> Note: The provided manifest will create an app named `mysql-connector` and attempt to bind the app to MySql service `myMySqlService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs mysql-connector`

On a Windows cell, you should see something like this during startup:

```bash
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running .\MySql
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://mysql-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple rows into the bound MySql database. To display those rows click on the `MySql Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information
