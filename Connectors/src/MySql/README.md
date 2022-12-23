# MySQL Connector Sample App - MySqlConnection

ASP.NET Core sample app illustrating how to use [Steeltoe MySQL Connector](https://docs.steeltoe.io/api/v3/connectors/mysql.html) for connecting to a MySQL service on CloudFoundry.
This sample illustrates using a `MySqlConnection` to issue commands to the bound database. There is also an additional sample that illustrates how to use Entity Framework Core.

## General prerequisites

1. Installed .NET Core SDK

## Running locally

1. Started MySQL [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)

## Running on CloudFoundry

1. Installed VMware CloudFoundry (optionally with Windows support)

## Create MySQL Service Instance on CloudFoundry

You must first create an instance of the MySQL service in an org/space.

1. `cf target -o your-org -s your-space`
1. `cf create-service p.mysql db-small myMySqlService`

## Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
1. `cd samples/Connectors/src/MySql`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       * `dotnet restore --configfile nuget.config`
       * `dotnet publish -r win-x64 --self-contained`
     - Push the app using the appropriate manifest:
       * `cf push -f manifest-windows.yml -p bin/Debug/net7.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       * `cf push -f manifest.yml`

> Note: The provided manifest will create an app named `mysql-connector` and attempt to bind the app to MySQL service `myMySqlService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs mysql-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running .\MySql
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://mysql-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple of rows into the bound MySQL database. They are displayed on the home page.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
