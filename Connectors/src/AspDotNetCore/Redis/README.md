# Redis Connector Sample App

This ASP.NET Core sample app uses the [Steeltoe Redis Connector](https://steeltoe.io/docs/steeltoe-connectors/#5-0-redis) to connect to Redis on Cloud Foundry.

This sample uses both [Microsoft RedisCache](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis) and [StackExchange `ConnectionMultiplexer`](https://github.com/StackExchange/StackExchange.Redis) to work with the same Redis service.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed Redis Cache service
1. Installed .NET Core SDK

## Deploy Redis and Sample to CloudFoundry

```
$ st init
$ st add app foo-sample
$ st add redis myRedisService
$ st target cloud-foundry
$ st deploy
> Note: The provided manifest will create an app named `foo-sample` and attempt to bind the app to Redis service `myRedisService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs foo-sample`

On a Windows cell, you should see something like this during startup:

```bash
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running .\Redis
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

Upon startup the app inserts a key/values into the bound Redis Cache.

This sample will be available at <http://foo-sample.[your-cf-apps-domain]/>.

To display those values click on the `Cache Data` link in the menu and you should see the key/values displayed using the Microsoft RedisCache.

You can click on the `ConnectionMultiplexer Data` link to view data using the StackExchange CollectionMultiplexer.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information
