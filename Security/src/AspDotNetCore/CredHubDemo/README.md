# CredHub Sample App

ASP.NET Core sample app illustrating how to use the Steeltoe [CredHub Api Client](https://github.com/SteeltoeOSS/Security) for generating, storing and interpolating credentials with [CredHub](https://github.com/cloudfoundry-incubator/credhub) running on Pivotal Cloud Foundry 2.0+.

> Due to complexities around running a local CredHub server, this application is not expected to work locally.

## Pre-requisites - CloudFoundry

1. Pivotal Cloud Foundry 2.0+
1. Deployed, accessible CredHub Server
1. Install .NET Core SDK
1. [CloudFoundry UAA Command Line Client](https://github.com/cloudfoundry/cf-uaac) installed

## Create UAA Client to use with CredHub

We will need to use the UAA command line tool to establish some security credentials for our sample app. Choose one of the provided `credhub-setup` scripts in the folder `samples/Security/scripts` to target your Cloud Foundry environment and create a UAA client with permissions to read and write in CredHub.

> Note: If you choose to change the values for UAA_CLIENT_ID or UAA_CLIENT_SECRET, be sure to update the credentials in appsettings.json

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Security/src/CredHubDemo
1. dotnet restore
1. Publish app to a directory selecting the framework and runtime you want to run on.
    * `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`
    * `dotnet publish -f netcoreapp2.1 -r win10-x64`
    * `dotnet publish -f net461 -r win10-x64`
1. Push the app using the appropriate manifest.
    * `cf push -f manifest-nix.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish`
    * `cf push -f manifest-win-core.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`
    * `cf push -f manifest-win.yml -p bin/Debug/net461/win10-x64/publish`

> Note: The provided manifest will create an app named `CredHubDemo-nix`, `CredHubDemo-win` or `CredHubDemo-wincore`.

## What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs credhubdemo-nix`

You should see something like this during startup:

```text
2016-08-05T07:23:02.15-0600 [CELL/0]     OUT Creating container
2016-08-05T07:23:03.81-0600 [CELL/0]     OUT Successfully created container
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Hosting environment: development
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Content root path: C:\containerizer\75E10B9301D2D9B4A8\user\app
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-05T07:23:14.68-0600 [APP/0]      OUT Now listening on: http://*:51217
```

At this point the app is up and running.  You can access it at <https://credhubdemo-nix.`YOUR-CLOUDFOUNDRY-APP-DOMAIN`/.>

Loading the home page of the app will generate and then delete a new password. A request to the Interpolate page will write a Json credential to CredHub and then use the Interpolate endpoint to inject that credential into a simulated VCAP:SERVICES that was set by the app at startup.

At startup, this application adds a CredHub client to the injection pipeline. The client expects the CredHub server to be located at <https://credhub.service.cf.internal:8844>. Should you need to override the CredHub Url, use your preferred means of application configuration to override the value at `CredHubClient:CredHubUrl`.

> Note: the CredHub Client will retrieve the address of the UAA server from the CredHub server's `/info` endpoint. Should that address prove inaccessible, you may override it by setting the environment variable `UAA_Server_Override`

---

### See the Official [Steeltoe Security Documentation](https://steeltoe.io/docs/steeltoe-security) for a more in-depth walkthrough of the samples and more detailed information