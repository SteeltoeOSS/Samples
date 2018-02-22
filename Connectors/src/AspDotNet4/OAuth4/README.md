# OAuth Connector Sample App

This ASP.NET 4.6.1 sample app uses the [Steeltoe OAuth Connector](https://steeltoe.io/docs/steeltoe-connectors/#6-0-oauth) to bind to a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) and expose the service's configuration data to the application.

This sample uses Autofac 4.0 for IoC services.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Installed Windows support

## Create OAuth2 Service Instance on CloudFoundry

You must first create an instance of a OAuth2 service in a org/space. As mentioned above there are a couple to choose from. In this example we will use the [UAA Server](https://github.com/cloudfoundry/uaa) as an OAuth2 service. To do this, we create a User Provided service providing the appropriate UAA server configuration data. You can use the provided `oauth.json` file in creating your User Provided service. Note that you will likely have to modify its contents to match your CloudFoundry setup.

1. cf target -o myorg -s development
1. cf cups myOAuthService -p oauth.json

If you want to use the [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) service for your OAuth2 server, follow the installation and configuration instructions [here](https://docs.pivotal.io/p-identity/installation.html).

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select OAuth4 project in Solution Explorer.
1. Right-click and select Publish
1. Use `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cd samples\Connectors\src\AspDotNet4\OAuth4
> cf push -p bin\Debug\net461\win10-x64\publish
```

> Note: The provided manifest(s) will create an app named `oauth-connector-4x` and attempt to bind the app the User Provided service `myOAuthService`.

## What to expect - CloudFoundry

Use the Cloud Foundry CLI to see the logs as you startup and use the app, with the command `cf logs oauth-connector-4x`

This sample will be available at <http://oauth-connector-4x.[your-cf-apps-domain]/>.

On the apps menu, click on the `OAuth Options` menu item and you should see meaningful configuration data for the bound OAuth service.

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-connectors) for guided tour of the samples and more detailed Connector information