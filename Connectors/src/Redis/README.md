# Redis Connector Sample App
ASP.NET Core sample app illustrating how to use [SteelToe Redis Connector](https://github.com/SteelToeOSS/Connectors/tree/master/src/SteelToe.CloudFoundry.Connector.Redis) for connecting a [RedisCache](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis) to a Redis Cache service on CloudFoundry.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7
2. Installed DiegoWindows support 
3. Installed Redis Cache marketplace service
4. Install .NET Core SDK
5. Web tools installed and on PATH, (e.g. npm, gulp, etc).  

# Create Redis Service Instance on CloudFoundry
You must first create an instance of the Redis service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-redis shared-vm myRedisService 

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Connectors/src/Redis
3. dotnet restore --configfile nuget.config
4. Publish app to a directory  
(e.g. `dotnet publish --output $PWD/publish --configuration Release --runtime win7-x64`)
5. Push the app using the provided manifest.
 (e.g.  `cf push -f manifest-windows.yml -p $PWD/publish`)

Note: The provided manifest will create an app named `redis-connector` and attempt to bind to the the app to Redis service `myRedisService`.

Note: We have experienced this [problem](https://github.com/dotnet/cli/issues/3283) when using the RTM SDK and when publishing to a relative directory... workaround is to use full path.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs redis-connector`

On a Windows cell, you should see something like this during startup:
```
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running cmd /c .\Redis --server.urls http://*:%PORT%
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the app is up and running.  Upon startup the app inserts a couple key/values into the bound Redis Cache. To display those values click on the `Cache Data` link in the menu and you should see the key/values displayed.