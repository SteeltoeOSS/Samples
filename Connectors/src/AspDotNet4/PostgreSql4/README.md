# PostgreSQL Connector Sample App - NpgsqlConnection

This ASP.NET 4.6.1 sample app uses the [Steeltoe PostgreSQL Connector](https://steeltoe.io/docs/steeltoe-connectors/#2-0-postgresql) to connect to PostgreSQL on CloudFoundry. 

This sample uses `NpgsqlConnection` to work with the database and Autofac for IoC services.

This sample uses Autofac 4.0 for IOC services.

## Pre-requisites - CloudFoundry

1. Install Pivotal CloudFoundry
1. Install Windows support
1. Install PostgreSQL database service (e.g. EDB Postgres)

## Create PostgreSQL Service Instance on CloudFoundry

You must first create an instance of the PostgreSQL database service in a org/space.

``` bash
> cf target -o myorg -s development

# for EDB PostgreSQL:
> cf create-service EDB-Shared-PostgreSQL "Basic PostgreSQL Plan" myPostgres

# for CrunchyPostgres:
> cf create-service postgresql-9.5-odb small myPostgres -c '{"db_name":"EFCoreSample", "db_username": "steeltoe", "owner_name":"<your name>", "owner_email":"<your email>"}'
# or with escaped double quotes for Powershell
> cf create-service postgresql-9.5-odb small myPostgres -c '{\"db_name\":\"EFCoreSample\", \"db_username\": \"steeltoe\", \"owner_name\":\"<your name>\", \"owner_email\":\"<your email>\"}'
```

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select the PostgreSql4 project in Solution Explorer.
1. Right-click and select Publish
1. Use `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cd samples\Connectors\src\AspDotNet4\PostgreSql4
> cf push -p bin\Debug\net461\win10-x64\publish
```

> Note: The provided manifest(s) will create an app named `postgresql-connector-4x` and attempt to bind the app to PostgreSql service `myPostgres`.

## What to expect - CloudFoundry

Use the Cloud Foundry CLI to see the logs as you startup and use the app, with the command `cf logs postgresql-connector-4x`

This sample will be available at <http://postgresql-connector-4x.[your-cf-apps-domain]/>.

Upon startup, the app inserts two rows into the bound PostgreSQL database. To display those rows click on the `Postgres Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-connectors) for guided tour of the samples and more detailed Connector information