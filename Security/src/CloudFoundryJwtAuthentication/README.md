# CloudFoundry JWT Bearer Token Security Sample App 
ASP.NET Core Web API sample app illustrating how to make use of the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization against a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) using JWT Bearer tokens.

This sample illustrates how you can secure your web api endpoints using JWT Bearer tokens issued by the CloudFoundry UAA server. 

Note: This application is intended to be used in conjunction with the [CloudFoundrySingleSignon](https://github.com/SteeltoeOSS/Samples/tree/dev/Security/src/CloudFoundrySingleSignon) sample app.  You should FIRST get that sample up and running on CloudFoundry and then follow these instructions after that.

# Pre-requisites - CloudFoundry

1. [CloudFoundrySingleSignon](https://github.com/SteeltoeOSS/Samples/tree/dev/Security/src/CloudFoundrySingleSignon) already up and running

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Security/src/CloudFoundryJwtAuthentication
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on. 
(e.g. `dotnet publish  -f netcoreapp2.0 -r ubuntu.14.04-x64`)
5. Push the app using the appropriate manifest.
 (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish`)


Note: The provided manifest(s) will create an app named `jwtauth` and attempt to bind it to the CUPS service `myOAuthService`.  Note: The CUPS service: `myOAuthService` is created when you follow the instructions for [CloudFoundrySingleSignon](https://github.com/SteeltoeOSS/Samples/tree/dev/Security/src/CloudFoundrySingleSignon).

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs jwtauth`

On a Windows cell, you should see something like this during startup:
```
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running cmd /c .\CloudFoundryJwtAuthentication --server.urls http://*:%PORT%
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```
At this point the app is up and running.  To access it, you should use the app: [CloudFoundrySingleSignon](https://github.com/SteeltoeOSS/Samples/tree/dev/Security/src/CloudFoundrySingleSignon).
