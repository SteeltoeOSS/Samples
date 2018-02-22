# MySql Connector Sample App - MySqlConnection

This ASP.NET 4.6.1 sample app uses the [Steeltoe MySql Connector](https://steeltoe.io/docs/steeltoe-connectors/#1-0-mysql) to connect to MySql on CloudFoundry with [Connector/NET](https://dev.mysql.com/downloads/connector/net/).

This sample uses `MySqlConnection` to work with the database and Autofac for IoC services.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
1. Installed Windows support
1. Installed MySql marketplace service

## Create MySql Service Instance on CloudFoundry

You must first create an instance of the MySql service in a org/space.

1. `cf target -o myorg -s development`
1. `cf create-service p-mysql 100mb myMySqlService`

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select MySql4 project in Solution Explorer.
1. Right-click and select Publish
1. Use `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cd samples\Connectors\src\AspDotNet4\MySql4
> cf push -p bin\Debug\net461\win10-x64\publish
```

> Note: The provided manifest(s) will create an app named `mysql-connector-4x` and attempt to bind the app to MySQL service `myMySqlService`.

## What to expect - CloudFoundry

Use the Cloud Foundry CLI to see the logs as you startup and use the app, with the command `cf logs mysql-connector-4x`

This sample will be available at <http://mysql-connector-4x.[your-cf-apps-domain]/>.

Upon startup, the app inserts two rows into the bound MySql database. To display those rows click on the `MySql Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-connectors) for guided tour of the samples and more detailed Connector information