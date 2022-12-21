# Redis Connector Sample App

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BConnectors_Redis%5D?branchName=main)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=20&branchName=main)

This ASP.NET Core sample app uses the [Steeltoe Redis Connector](https://docs.steeltoe.io/api/v3/connectors/redis.html) to connect to Redis on Cloud Foundry.

This sample uses both [Microsoft RedisCache](https://learn.microsoft.com/dotnet/api/microsoft.extensions.caching.redis.rediscache) and [StackExchange `ConnectionMultiplexer`](https://github.com/StackExchange/StackExchange.Redis) to work with the same Redis service.

## General Pre-requisites

1. Installed .NET Core SDK

## Running Locally

1. Installed Redis Server
1. Set [ASPNETCORE_ENVIRONMENT=Development] (<https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments>)

## Running on CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed Redis Cache service

## Create Redis Service Instance on CloudFoundry

You must first create an instance of the Redis service in an org/space.

1. `cf target -o myorg -s development`
1. `cf create-service p-redis shared-vm myRedisService`

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/Redis`
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

> Note: The provided manifest will create an app named `redis-connector` and attempt to bind the app to Redis service `myRedisService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs redis-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running .\Redis
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

Upon startup the app inserts key/value pairs into the bound Redis Cache.

This sample will be available at <http://redis-connector.[your-cf-apps-domain]/>.

To display those values click on the `Cache Data` link in the menu and you should see the key/value pairs displayed using the Microsoft RedisCache.

You can click on the `ConnectionMultiplexer Data` link to view data using the StackExchange CollectionMultiplexer.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
