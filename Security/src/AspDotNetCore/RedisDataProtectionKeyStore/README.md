# ASP.NET Core DataProtection Redis Keystore Sample App

ASP.NET Core sample app illustrating how to make use of the Steeltoe [DataProtection Key Storage Provider for Redis](https://github.com/SteeltoeOSS/Security). Simplifies using a Redis cache on CloudFoundry for storing DataProtection keys.

## Pre-requisites - CloudFoundry

1. Installed Pivotal Cloud Foundry
1. Optionally, installed Windows support on Cloud Foundry
1. Installed Redis Cache marketplace service
1. Install .NET Core SDK

## Create Redis Service Instance on CloudFoundry

You must first create an instance of the Redis service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-redis shared-vm myRedisService

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Security/src/RedisDataProtectionKeyStore
1. dotnet restore --configfile nuget.config
1. Publish app to a directory, specifying the desired framework and runtime:
    * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
    * `dotnet publish -f netcoreapp2.1 -r win10-x64`
1. Push the app using the appropriate manifest:
    * `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
    * `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`

> Note: The provided manifest will create an app named `keystore` and attempt to bind to the Redis service `myRedisService`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

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

At this point the app is up and running. Bring up the home page of the app and click on the `Protected` link in the menu and you should see something like the following:

```bash
Protected Data.
InstanceIndex=0
SessionId=989f8693-b43b-d8f0-f48f-187460f2aa02
ProtectedData=My Protected String - 6f954faa-e06d-41b9-b88c-6e387a921420
```

At this point the app has created a new Session with the ProtectedData encrypted and saved in the Session.

Next, scale the app to multi-instance (eg. `cf scale keystore -i 2`). Wait for the new instance to startup.

Using the same browser session, click on the `Protected` menu item a couple more times. It may take a couple clicks to get routed to the second app instance. When this happens, you should see the InstanceId changing but the SessionId and the ProtectedData remaining the same.

A couple things to note at this point about this app:

* The app is using the CloudFoundry Redis service to store session data.  As a result, the session data is available to all instances of the app.
* The `session handle` that is in the session cookie and the data that is stored in the session in Redis is encrypted using keys that are now stored in the keyring which is also stored in the CloudFoundry Redis service. So when you scale the app to multiple instances the same keyring is used by all instances and therefore the `session handle` and the session data can be decrypted by any instance of the application.

---

### See the Official [Steeltoe Security Documentation](https://steeltoe.io/docs/steeltoe-security) for a more in-depth walkthrough of the samples and more detailed information