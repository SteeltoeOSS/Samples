# Postgres Connector Sample App - NpgsqlConnection
ASP.NET Core sample app illustrating how to use [SteelToe Postgres Connector](https://github.com/SteelToeOSS/Connectors/tree/master/src/SteelToe.CloudFoundry.Connector.PostgreSql) for connecting to a Postgres database service on CloudFoundry using [Npgsql - 3.1.5](https://www.npgsql.org/). This specific sample illustrates how to use a `NpgsqlConnection` to issue commands to the bound database. There is also an additional sample which illustrates how to use EFCore.

# Pre-requisites - CloudFoundry

1. Install Pivotal CloudFoundry 1.7
2. Install Postgres database service (e.g. EDB Postgres)
3. Install .NET Core SDK
4. Web tools installed and on Path. If you have VS2015 Update 3 installed then add this to your path: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Web\External


# Create Postgres Service Instance on CloudFoundry
You must first create an instance of the Postgres database service in a org/space.

1. cf target -o myorg -s development
2. cf create-service EDB-Shared-PostgreSQL "Basic PostgreSQL Plan" myPostgres

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Connectors/src/PostgreSql
3. dotnet restore --configfile nuget.config
4. Publish app to a directory  
(e.g. `dotnet publish --output $PWD/publish --configuration Release --framework net451 --runtime win7-x64`)
5. Push the app using the provided manifest.
 (e.g.  `cf push -f manifest-windows.yml -p $PWD/publish` or `cf push -f manifest.yml -p $PWD/publish` )

Note: The provided manifest(s) will create an app named `postgres-connector` and attempt to bind to the the app to PostgreSql service `myPostgres`.

Note: We have experienced this [problem](https://github.com/dotnet/cli/issues/3283) when using the RTM SDK and when publishing to a relative directory... workaround is to use full path.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs postgres-connector`

On a Windows cell, you should see something like this during startup:
```
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running cmd /c .\PostgreSql --server.urls http://*:%PORT%
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```
At this point the app is up and running.  Upon startup the app inserts a couple rows into the bound Postgres database. To display those rows click on the `Postgres Data` link in the menu and you should see the row data displayed.