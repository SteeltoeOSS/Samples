# PostgreSQL Connector Sample App - EntityFramework Core

An ASP.NET Core sample application for the [Steeltoe PostgreSQL Connector](https://steeltoe.io/docs/steeltoe-connectors/#2-0-postgresql).

This sample uses EntityFramework Core to issue commands to the bound database.
There is another sample using [NpgsqlConnection](./PostgreSql).

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed PostgreSQL database service (e.g. EDB Postgres or CrunchyPostgreSQL)
1. Installed .NET Core SDK

## Create PostgreSQL Service Instance on CloudFoundry

Create an instance of the PostgreSQL database service in a org/space:

``` bash
> cf target -o myorg -s development

# for EDB PostgreSQL:
> cf create-service EDB-Shared-PostgreSQL "Basic PostgreSQL Plan" myPostgres

# for CrunchyPostgres:
> cf create-service postgresql-9.5-odb small myPostgres -c '{"db_name":"postgresample", "db_username": "steeltoe", "owner_name":"<your name>", "owner_email":"<your email>"}'
# or with escaped double quotes for Powershell
> cf create-service postgresql-9.5-odb small myPostgres -c '{\"db_name\":\"postgresample\", \"db_username\": \"steeltoe\", \"owner_name\":\"<your name>\", \"owner_email\":\"<your email>\"}'
```

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/AspDotNetCore/PostgreEFCore`
1. `dotnet restore --configfile nuget.config`
1. Publish app to a local directory, specifying the framework and runtime (select ONE of these commands):
   * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
   * `dotnet publish -f net461 -r win10-x64`
1. Push the app using the appropriate manifest (select ONE of these commands):
   * `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
   * `cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish`

> Note: The provided manifest(s) will create an app named `postgresefcore-connector` and attempt to bind the app to PostgreSql service `myPostgres`.

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

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information