# MySql Connector Sample App - EntityFramework Core
ASP.NET Core sample app illustrating how to use the EntityFramework Core together with [Steeltoe MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) and [Connector/NET - 7.0.x](https://dev.mysql.com/downloads/connector/net/) for connecting to a MySql service on CloudFoundry. There is also an additional sample which illustrates how to use a `MySqlConnection` to issue commands to the bound database.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
2. Installed MySql marketplace service
3. Install .NET Core SDK
4. Web tools installed and on Path. If you have VS2015 Update 3 installed then add this to your path: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Web\External

# Create MySql Service Instance on CloudFoundry
You must first create an instance of the MySql service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-mysql 100mb myMySqlService 

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Connectors/src/MySqlCore
3. dotnet restore --configfile nuget.config
4. Publish app to a directory  
(e.g. `dotnet publish --output $PWD/publish --configuration Release --framework netcoreapp1.1 --runtime ubuntu.14.04-x64`)
5. Push the app using the provided manifest.
 (e.g.  `cf push -f manifest.yml -p $PWD/publish`)

Note: The provided manifest will create an app named `mysqlefcore-connector` and attempt to bind to the the app to MySql service `myMySqlService`.


# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs mysqlefcore-connector`

On a Windows cell, you should see something like this during startup:
```
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path:  /home/vcap/app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:8080
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the app is up and running.  Upon startup the app inserts a couple rows into the bound MySql database. To display those rows click on the `MySql Data` link in the menu and you should see the row data displayed.
