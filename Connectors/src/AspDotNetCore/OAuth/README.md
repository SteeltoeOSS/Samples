# OAuth Connector Sample App - 
ASP.NET Core sample app illustrating how to use the OAuth Connector to automatically bind to a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) and expose the services configuration data as injectable ASP.NET Core `IOptions`.
This connector will typically be used in conjunction with the ASP.NET Core [CloudFoundry External Security Providers](https://github.com/SteeltoeOSS/Security).
# Pre-requisites - CloudFoundry

1. Install Pivotal CloudFoundry
2. Optionaly, installed Windows support (Greenhouse)  
3. Install .NET Core SDK

# Create OAuth2 Service Instance on CloudFoundry
You must first create an instance of a OAuth2 service in a org/space. As mentioned above there are a couple to choose from. In this example we will use the [UAA Server](https://github.com/cloudfoundry/uaa) as an OAuth2 service. To do this, we create a CUPS service providing the appropriate UAA server configuration data. You can use the provided `oauth.json` file in creating your CUPS service. Note that you will likely have to modify its contents to match your CloudFoundry setup.

1. cf target -o myorg -s development
2. cf cups myOAuthService -p oauth.json

If you want to use the [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) service for your OAuth2 server, follow the installation and configuration instructions [here](https://docs.pivotal.io/p-identity/installation.html).

# Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Connectors/src/AspDotNetCore/OAuth
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on. 
(e.g. `dotnet publish  -f netcoreapp2.0 -r ubuntu.14.04-x64`)
5. Push the app using the appropriate manifest.
 (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish`)


Note: The provided manifest(s) will create an app named `oauth` and attempt to bind to the the app the CUPS service `myOAuthService`.

# What to expect - CloudFoundry
After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs oauth`

On a Windows cell, you should see something like this during startup:
```
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running cmd /c .\OAuth --server.urls http://*:%PORT%
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```
At this point the app is up and running.  You can access the app at https://oauth.x.y.z/.  On the apps menu, click on the `OAuth Options` menu item and you should see meaningful configurtion data for the bound OAuth service.
