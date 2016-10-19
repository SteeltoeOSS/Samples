# ASP.NET Core DataProtection Redis Keystore Sample App 
ASP.NET Core sample app illustrating how to make use of the Steeltoe [DataProtection Key Storage Provider for Redis](https://github.com/SteeltoeOSS/Security). Simplifies using a Redis cache on CloudFoundry for storing DataProtection keys.

# Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry 1.7+
2. Installed DiegoWindows support (Greenhouse) 
3. Installed Redis Cache marketplace service
4. Install .NET Core SDK
5. Web tools installed and on Path. If you have VS2015 Update 3 installed then add this to your path: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Web\External

# Create Redis Service Instance on CloudFoundry
You must first create an instance of the Redis service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-redis shared-vm myRedisService
 
# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Security/src/RedisDataProtectionKeyStore
3. dotnet restore --configfile nuget.config
4. Publish app to a directory  
(e.g. `dotnet publish --output $PWD/publish --configuration Release --framework net451 --runtime win7-x64`)
5. Push the app using the provided manifest.
 (e.g.  `cf push -f manifest-windows.yml -p $PWD/publish`)

Note: The provided manifest will create an app named `keystore` and attempt to bind to the Redis service `myRedisService`.

Note: We have experienced this [problem](https://github.com/dotnet/cli/issues/3283) when using the RTM SDK and when publishing to a relative directory... workaround is to use full path.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs keystore`

On a Windows cell, you should see something like this during startup:
```
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running cmd /c .\RedisDataProtectionKeyStore --server.urls http://*:%PORT%
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```
At this point the app is up and running.  Click on the `Protected` link in the menu and you should see something like the following:
```
Protected Data.
InstanceIndex=0
SessionId=989f8693-b43b-d8f0-f48f-187460f2aa02
ProtectedData=My Protected String - 6f954faa-e06d-41b9-b88c-6e387a921420
```
At this point the app has created a new Session with the ProtectedData encrypted and saved in the Session.

Next, scale the app to multi-instance (eg. `cf scale keystore -i 2`). Wait for the new instance to startup.

Using the same browser session, click on the `Protected` menu item a couple more times. It may take a couple clicks to get routed to the second app instance. When this happens, you should see the InstanceId changing but the SessionId and the ProtectedData remaining the same.