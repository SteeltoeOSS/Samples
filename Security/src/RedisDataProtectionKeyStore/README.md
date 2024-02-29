# ASP.NET Core DataProtection Redis Keystore Sample App

ASP.NET Core sample app illustrating how to make use of the Steeltoe [DataProtection Key Storage Provider for Redis](https://github.com/SteeltoeOSS/Security). Simplifies using a Redis cache on CloudFoundry for storing DataProtection keys.

## General prerequisites

1. Installed .NET Core SDK

## Running locally

1. Started Redis [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)

## Running on CloudFoundry

Pre-requisites:

1. Installed CloudFoundry (optionally with Windows support)
1. Installed Redis Cache marketplace service (configured to enable LUA scripts)

### Create Redis Service Instance on CloudFoundry

You must first create an instance of the Redis service in an org/space.

1. `cf target -o your-org -s your-space`
1. `cf create-service p.redis on-demand-cache myRedisService`

### Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
1. `cd samples/Security/src/RedisDataProtectionKeyStore`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       - `dotnet restore --configfile nuget.config`
       - `dotnet publish -r win-x64 --self-contained`
     - Push the app using the appropriate manifest:
       - `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       - `cf push -f manifest.yml`

> Note: The provided manifest will create an app named `keystore` and attempt to bind the app to Redis service `myRedisService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs keystore`

On a Windows cell, you should see something like this during startup:

```bash
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running .\RedisDataProtectionKeyStore
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://keystore.[your-cf-apps-domain]/>.

Upon startup, the app displays session information on the home page, something like the following:

| Instance Index | Session ID | Session Value |
|---|---|---|
| 0 | 989f8693-b43b-d8f0-f48f-187460f2aa02 | Example Protected String - 3a04ea4e-1393-4ff5-9fc7-9f7201dd95ad |

At this point, the app has created a new ASP.NET session containing the encrypted session value.

Next, scale the app to multi-instance (eg. `cf scale keystore -i 2`). Wait for the new instance to startup.

Using the same browser session, refresh the page a couple more times.
It may take a few tries to get routed to the second app instance.
When this happens, you should see the `Instance Index` changing, while the `Session ID` and `Session Value` remain the same.

A couple things to note at this point about this app:

* The app is using the CloudFoundry Redis service to store session data. As a result, the session state is available to all instances of the app.
* The session handle that is in the session cookie and the data that is stored in the session in Redis is encrypted using keys that are now stored in the keyring,
which is also stored in the CloudFoundry Redis service. So when you scale the app to multiple instances, the same keyring is used by all instances
and therefore the `session handle` and the session data can be decrypted by any instance of the application.
* For multiple app instances to share Redis data, ensure they have an identical `name` in the connection string in `appsettings.json`.

---

### See the Official [Steeltoe Security Documentation](https://steeltoe.io/docs/steeltoe-security) for a more in-depth walkthrough of the samples and more detailed information.
