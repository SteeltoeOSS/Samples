# RedisCloudfoundry 
ASP.NET Core sample app illustrating how to use [Redis on CloudFoundry](https://docs.pivotal.io/redis/index.html) as a distributed cache in ASP.NET Core.


# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7
2. Installed Redis for CloudFoundry 1.5.13
3. Install .NET Core SDK
4. Web tools installed and on PATH, (e.g. npm, gulp, etc).  

# Setup Service Registry on CloudFoundry
You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-redis shared-vm myRedisService 

# Publish App & Push to CloudFoundry

NOTE: The Redis client currently is supported ONLY on Windows and ONLY on .NET 4.5.1+.  Microsoft/StackExchange are currently working on support for .NET Core and once complete we will add also.

1. cf target -o myorg -s development
2. cd samples/Caching/src/RedisCloudFoundry
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on.
(e.g. `dotnet publish --output $PWD/publish --configuration Release --framework net451 --runtime win7-x64`)
5. Push the app using the appropriate manifest.
 (e.g. `cf push -f manifest.yml -p $PWD/publish` or `cf push -f manifest-windows.yml -p $PWD/publish`)

Note: We have experienced this [problem](https://github.com/dotnet/cli/issues/3283) when using the RC2 SDK and when publishing to a relative directory... workaround is to use full path.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs redis-sample`

On a Windows cell, you should see something like this during startup:
```
2016-06-03T10:21:40.10-0600 [APP/0]      OUT Running cmd /c .\RedisCloudFoundry --server.urls http://*:%PORT%
2016-06-03T10:21:43.45-0600 [APP/0]      OUT Hosting environment: development
2016-06-03T10:21:43.45-0600 [APP/0]      OUT Now listening on: http://*:50485
2016-06-03T10:21:43.45-0600 [APP/0]      OUT Content root path: C:\containerizer\70EE51AA8BABB1870B\user\app
2016-06-03T10:21:43.45-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the Sample app is up and running and ready.  

If you hit https://redis-sample.x.y.z/ you should see the typical ASP.NET Core UI appear. Select the `Cache Data` menu item. You should see:
```
Cache Data.
Key1=Key1Value
Key2=Key2Value
```
This data is loaded into the Redis cache upon startup and then fetched in the Controller method `CacheData()`.