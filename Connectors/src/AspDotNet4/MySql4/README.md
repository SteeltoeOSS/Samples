# MySql Connector Sample App - MySqlConnection
ASP.NET 4.x sample app illustrating how to use [Steeltoe MySql Connector](https://github.com/SteeltoeOSS/Connectors/tree/master/src/Steeltoe.CloudFoundry.Connector.MySql) for connecting to a MySql service on CloudFoundry using [Connector/NET - 7.0.x](https://dev.mysql.com/downloads/connector/net/). This specific sample illustrates how to use a `MySqlConnection` to issue commands to the bound database. 

This sample makes use of Autofac 4.0 for IOC services.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
2. Installed DiegoWindows support (Greenhouse)
3. Installed MySql marketplace service

# Create MySql Service Instance on CloudFoundry
You must first create an instance of the MySql service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-mysql 100mb myMySqlService
 
# Publish App & Push to CloudFoundry

1. Open Samples\Discovery\Discovery.sln in Visual Studio 2015.
2. Select MySql4 project in Solution Explorer.
3. Right-click and select Publish
4. Publish the App to a folder. (e.g. c:\publish)
5. cd publish_folder (e.g. cd c:\publish)
6. cf push 

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs mysql4-connector`

You should see something like this during startup:
```
2016-12-05T11:26:33.12-0700 [STG/0]      OUT Successfully destroyed container
2016-12-05T11:26:34.60-0700 [CELL/0]     OUT Successfully created container
2016-12-05T11:26:40.49-0700 [APP/0]      OUT Running ..\tmp\lifecycle\WebAppServer.exe
2016-12-05T11:26:40.55-0700 [APP/0]      OUT PORT == 52222
2016-12-05T11:26:40.56-0700 [APP/0]      OUT 2016-12-05 18:26:40Z|INFO|Port:52222
2016-12-05T11:26:40.56-0700 [APP/0]      OUT 2016-12-05 18:26:40Z|INFO|Webroot:C:\containerizer\D01F08F4D6E6E541C6\user\app
2016-12-05T11:26:40.63-0700 [APP/0]      OUT 2016-12-05 18:26:40Z|INFO|Starting web server instance...
2016-12-05T11:26:40.74-0700 [APP/0]      OUT Server Started.... press CTRL + C to stop
2016-12-05T11:26:46.50-0700 [APP/0]      OUT Initializing data
2016-12-05T11:26:48.16-0700 [APP/0]      OUT Initializing data complete!
```
At this point the app is up and running.  Upon startup the app inserts a couple rows into the bound MySql database. To display those rows click on the `MySql Data` link in the menu and you should see the row data displayed.
