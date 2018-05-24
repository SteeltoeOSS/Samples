# MySql Connector Sample App - EntityFramework Core

ASP.NET Core sample app illustrating how to use the EntityFramework Core together with [Steeltoe MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) for connecting to a MySql service on CloudFoundry. There is also an additional sample which illustrates how to use a `MySqlConnection` to issue commands to the bound database.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed MySql marketplace service
1. Installed .NET Core SDK

## Create MySql Service Instance on CloudFoundry

You must first create an instance of the MySql service in a org/space.

1. `cf target -o myorg -s development`
1. `cf create-service p.mysql 100-mb myMySqlService`

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/AspDotNetCore/MySqlEFCore`
1. `dotnet restore --configfile nuget.config`
1. Publish app to a local directory, specifying the framework and runtime (select ONE of these commands):
   * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
   * `dotnet publish -f net461 -r win10-x64`
1. Push the app using the appropriate manifest (select ONE of these commands):
   * `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
   * `cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish`

> Note: The provided manifest will create an app named `mysqlefcore-connector` and attempt to bind the app to MySql service `myMySqlService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs mysqlefcore-connector`

On a Windows cell, you should see something like this during startup:

```bash
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path:  /home/vcap/app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:8080
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://mysqlefcore-connector.[your-cf-apps-domain]/>.

Upon startup the app should create a table named `EFCoreTestData` and insert two rows. To display those rows, click on the `MySql Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information