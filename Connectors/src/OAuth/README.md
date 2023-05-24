# OAuth Connector Sample App

ASP.NET Core sample app illustrating how to use [Steeltoe OAuth Connector](https://docs.steeltoe.io/api/v3/connectors/oauth.html) to bind to a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service/)) and expose the service's configuration data as ASP.NET Core `IOptions`.
This connector will typically be used in conjunction with the ASP.NET Core [CloudFoundry External Security Providers](https://docs.steeltoe.io/api/v3/security/).

## General prerequisites

1. Installed .NET Core SDK

## Running locally

No installation required.

## Running on CloudFoundry

Pre-requisites:

1. Installed CloudFoundry (optionally with Windows support)
1. OAuth2 Service Instance

## Create OAuth2 Service Instance on CloudFoundry

You must first create an instance of a OAuth2 service in an org/space.
As mentioned above, there are two to choose from.
This example uses the [UAA Server](https://github.com/cloudfoundry/uaa).
To do this, create a user-provided service with the appropriate UAA server configuration data.
You can use the provided `oauth.json` file in creating your user-provided service after modifying its contents to match your CloudFoundry environment.

1. `cf target -o your-org -s your-space`
1. `cf cups myOAuthService -p oauth.json`

If you want to use the [Single Sign-On for VMware Tanzu Application Service](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service/) service for your OAuth2 server, follow the installation and configuration instructions [here](https://docs.vmware.com/en/Single-Sign-On-for-VMware-Tanzu-Application-Service/1.14/sso/GUID-installation.html).

## Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
1. `cd samples/Connectors/src/OAuth`
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

> Note: The provided manifest(s) will create an app named `oauth-connector` and attempt to bind the app to user-provided service `myOAuthService`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs oauth-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:09.07-0600 [APP/0]      OUT Running .\OAuth
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

This sample will be available at <http://oauth-connector.[your-cf-apps-domain]/>.

Upon startup, the app displays configuration data for the bound OAuth service on the home page.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
