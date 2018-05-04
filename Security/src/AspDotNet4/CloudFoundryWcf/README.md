# WCF Sample App for Cloud Foundry Security

Legacy ASP.NET/WCF sample app using the Steeltoe [CloudFoundry External Security Provider](https://github.com/SteeltoeOSS/Security) for Authentication and Authorization against a CloudFoundry OAuth2 security service (e.g. [UAA Server](https://github.com/cloudfoundry/uaa) or [Pivotal Single Signon](https://docs.pivotal.io/p-identity/)) using JWT Bearer tokens.

This sample illustrates how you can secure your WCF services using JWT Bearer tokens issued by the CloudFoundry UAA server.

> Note: This application is intended to be used in conjunction with the [CloudFoundrySingleSignon][sso] sample app. You should FIRST get that sample up and running on CloudFoundry and follow these instructions after that.

## Pre-requisites - CloudFoundry

1. [CloudFoundrySingleSignon][sso] already up and running

## Publish App & Push to CloudFoundry

1. Open Samples\Security\src\AspDotNet4\4x-Security.sln in Visual Studio
1. Right click on the CloudFoundryWcf project, select "Publish"
1. Use the included `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cf target -o myorg -s development
> cd samples/Security/src/AspNet4/CloudFoundryJwtAuthentication
> cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
```

The provided manifest(s) will create an app named `wcf-jwt-4x` and attempt to bind it to `mySSOService`. Alter the manifest to bind to `myOAuthService` if you are using UAA instead of the SSO tile.

> Note: `mySSOService` is created when you follow the Pivotal SSO instructions for [CloudFoundrySingleSignon][sso].

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs wcf-jwt-4x`

On a Windows cell, you should see something like this during startup:

```bash
2018-05-04T10:21:57.606-05:00 [CELL/0] [OUT] Creating container
2018-05-04T10:21:58.125-05:00 [CELL/0] [OUT] Successfully destroyed container
2018-05-04T10:21:58.725-05:00 [CELL/0] [OUT] Successfully created container
2018-05-04T10:22:01.108-05:00 [CELL/0] [OUT] Starting health monitoring of container
2018-05-04T10:22:02.303-05:00 [APP/PROC/WEB/0] [OUT] Server Started for dff521d0-8232-4b10-b884-65c549f8036f
2018-05-04T10:22:05.745-05:00 [CELL/0] [OUT] Container became healthy
```

At this point the app is up and running.  To access it, you should use the app: [CloudFoundrySingleSignon][sso].

[sso]: ../CloudFoundrySingleSignon

---

### See the Official [Steeltoe Security Documentation](https://steeltoe.io/docs/steeltoe-security) for a more in-depth walkthrough of the samples and more detailed information