# PostgreSQL Connector Sample App - Entity Framework Core

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BConnectors_PostgreEFCore%5D?branchName=main)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=22&branchName=main)

ASP.NET Core sample app illustrating how to use Entity Framework Core together with [Steeltoe PostgreSQL Connector](https://docs.steeltoe.io/api/v3/connectors/postgresql.html)  for connecting to a PostgreSQL service on CloudFoundry. There is also an additional sample which illustrates how to use a `NpgsqlConnection` to issue commands to the bound database.

## General Pre-requisites

1. Installed .NET Core SDK

## Running Locally

1. Installed PostgreSQL Server
1. Created PostgreSQL database and user with appropriate access level
1. Set [ASPNETCORE_ENVIRONMENT=Development] (<https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments>)
1. Added your connection string to appsettings.development.json under Postgres:Client:ConnectionString

## Running on CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed PostgreSQL database service (e.g. EDB Postgres or CrunchyPostgreSQL)

## Create PostgreSQL Service Instance on CloudFoundry

You must first create an instance of the PostgreSQL service in an org/space.

``` bash
> cf target -o myorg -s development

# for EDB PostgreSQL:
> cf create-service EDB-Shared-PostgreSQL "Basic PostgreSQL Plan" myPostgres

# for CrunchyPostgres:
> cf create-service postgresql-10-odb standalone myPostgres -c '{"db_name":"postgresample", "db_username": "steeltoe", "owner_name":"<your name>", "owner_email":"<your email>"}'
# or with escaped double quotes for Powershell
> cf create-service postgresql-10-odb stadalone myPostgres -c '{\"db_name\":\"postgresample\", \"db_username\": \"steeltoe\", \"owner_name\":\"<your name>\", \"owner_email\":\"<your email>\"}'
```

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/PostgreEFCore`
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

> Note: The provided manifest(s) will create an app named `postgresefcore-connector` and attempt to bind the app to PostgreSQL service `myPostgres`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs postgresefcore-connector`

On a Windows cell, you should see something like this during startup:

```bash
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running .\PostgreEFCore
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

This sample will be available at <http://postgresefcore-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple rows into the bound PostgreSQL database. To display those rows, click on the `Postgres Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
