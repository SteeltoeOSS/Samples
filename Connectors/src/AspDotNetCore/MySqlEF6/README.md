# MySql Connector Sample App - EntityFramework 6
ASP.NET Core sample app illustrating how to use the EntityFramework 6 together with [Steeltoe MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) for connecting to a MySql service on CloudFoundry. There is also an additional sample which illustrates how to use a `MySqlConnection` to issue commands to the bound database.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 
2. Installed Windows support (Greenhouse)
3. Installed MySql CloudFoundry service
4. Install .NET Core SDK


# Create MySql Service Instance on CloudFoundry
You must first create an instance of the MySql service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-mysql 100mb myMySqlService 

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Connectors/src/AspDotNetCore/MySqlEF6
3. dotnet restore --configfile nuget.config
4. Publish app to a directory. 
(e.g. `dotnet publish -r win10-x64`)
5. Push the app using the provided manifest.
 (e.g. `cf push -f manifest-windows.yml -p bin/Debug/net46/win10-x64/publish`)


Note: The provided manifest will create an app named `mysqlef6-connector` and attempt to bind to the the app to MySql service `myMySqlService`.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs mysqlef6-connector`

On a Windows cell, you should see something like this during startup:
```
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running cmd /c .\MySqlEF6 --server.urls http://*:%PORT%
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the app is up and running.  Upon startup the app inserts a couple rows into the bound MySql database. To display those rows click on the `MySql Data` link in the menu and you should see the row data displayed.
