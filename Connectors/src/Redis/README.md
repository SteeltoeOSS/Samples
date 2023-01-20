# Redis Connector Sample App

ASP.NET Core sample app illustrating how to use [Steeltoe Redis Connector](https://docs.steeltoe.io/api/v3/connectors/redis.html) to connect to Redis on Cloud Foundry.
This sample uses both [Microsoft RedisCache](https://learn.microsoft.com/dotnet/api/microsoft.extensions.caching.redis.rediscache) and [StackExchange `ConnectionMultiplexer`](https://github.com/StackExchange/StackExchange.Redis) to work with the same Redis service.

## General prerequisites

1. Installed .NET Core SDK

## Running locally

1. Installed Redis Server

## Running on CloudFoundry

1. Installed CloudFoundry (optionally with Windows support)
1. Installed Redis Cache service

## Create Redis Service Instance on CloudFoundry

You must first create an instance of the Redis service in an org/space.

1. `cf target -o your-org -s your-space`
1. `cf create-service p-redis shared-vm myRedisService`

## Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
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

This sample will be available at <http://redis-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple of key/value pairs into the bound Redis cache. They are displayed on the home page.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
