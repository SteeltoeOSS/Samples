# Redis Connector Sample App

This ASP.NET 4.6.1 sample app uses the [Steeltoe Redis Connector](https://steeltoe.io/docs/steeltoe-connectors/#5-0-redis) to connect to Redis on Cloud Foundry.

This sample uses both [Microsoft RedisCache](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis) and [StackExchange `ConnectionMultiplexer`](https://github.com/StackExchange/StackExchange.Redis) to work with the same Redis service.

This sample uses Autofac 4.0 for IoC services.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Installed Windows support
1. Installed Redis Cache service

## Create Redis Service Instance on CloudFoundry

You must first create an instance of the Redis service in a org/space.

1. `cf target -o myorg -s development`
1. `cf create-service p-redis shared-vm myRedisService`

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select Redis4 project in Solution Explorer.
1. Right-click and select Publish
1. Use `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cd samples\Connectors\src\AspDotNet4\Redis4
> cf push -p bin\Debug\net461\win10-x64\publish
```

> Note: The provided manifest will create an app named `redis-connector-4x` and attempt to bind the app to Redis service `myRedisService`.

## What to expect - CloudFoundry

Use the Cloud Foundry CLI to see the logs as you startup and use the app, with the command `cf logs redis-connector-4x`

Upon startup, the app inserts a key/values into the bound Redis Cache.

This sample will be available at <http://redis-connector-4x.[your-cf-apps-domain]/>.

To display those values using the Microsoft RedisCache, click on the `Cache Data` link in the menu.

To view data using the StackExchange CollectionMultiplexer, click on the `ConnectionMultiplexer Data` link in the menu.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-connectors) for guided tour of the samples and more detailed Connector information