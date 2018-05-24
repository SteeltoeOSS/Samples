# OAuth Connector Sample App

This ASP.NET Core sample app uses the OAuth Connector to bind to a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) and expose the service's configuration data as ASP.NET Core `IOptions`.
This connector will typically be used in conjunction with the ASP.NET Core [CloudFoundry External Security Providers](https://github.com/SteeltoeOSS/Security).

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. (Optional) installed Windows support
1. Installed .NET Core SDK

## Create OAuth2 Service Instance on CloudFoundry

You must first create an instance of a OAuth2 service in a org/space. As mentioned above, there are two to choose from. This example uses the [UAA Server](https://github.com/cloudfoundry/uaa). To do this, create a User Provided service with the appropriate UAA server configuration data. You can use the provided `oauth.json` file in creating your User Provided service after modifying its contents to match your CloudFoundry environment.

1. `cf target -o myorg -s development`
1. `cf cups myOAuthService -p oauth.json`

If you want to use the [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) service for your OAuth2 server, follow the installation and configuration instructions [here](https://docs.pivotal.io/p-identity/installation.html).

## Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. `cd samples/Connectors/src/AspDotNetCore/OAuth`
1. `dotnet restore --configfile nuget.config`
1. Publish app to a local directory, specifying the framework and runtime (select ONE of these commands):
   * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
   * `dotnet publish -f net461 -r win10-x64`
1. Push the app using the appropriate manifest (select ONE of these commands):
   * `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
   * `cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish`

> Note: The provided manifest(s) will create an app named `oauth-connector` and attempt to bind the app to User Provided service `myOAuthService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs oauth-connector`

On a Windows cell, you should see something like this during startup:

```bash
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running .\OAuth
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

This sample will be available at <http://oauth-connector.[your-cf-apps-domain]/>.

On the app's menu, click on `OAuth Options` to see meaningful configuration data for the bound OAuth service.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-service-connectors) for a more in-depth walkthrough of the samples and more detailed information