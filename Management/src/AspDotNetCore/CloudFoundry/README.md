# Management Sample App for Cloud Foundry

ASP.NET Core sample app illustrating how to use [Steeltoe Management Endpoints](https://github.com/SteeltoeOSS/Management) together with the [Pivotal Apps Manager](https://docs.pivotal.io/pivotalcf/1-11/console/index.html) for monitoring and managing your applications on Cloud Foundry.

## Pre-requisites - CloudFoundry

1. Installed Pivotal Cloud Foundry
2. Installed Apps Manager on Cloud Foundry
3. Installed MySql CloudFoundry service
4. Install .NET Core SDK

## Create MySql Service Instance on CloudFoundry

You must first create an instance of the MySql service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-mysql 100mb myMySqlService

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Management/src/AspDotNetCore/CloudFoundry
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64`)
5. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish`)

> Note: The provided manifest will create an app named `actuator` and attempt to bind to the the app to MySql service `myMySqlService`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs actuator`

On a Windows cell, you should see something like this during startup:

```bash
2017-08-17T08:48:18.97-0600 [CELL/0] OUT Creating container
2017-08-17T08:48:19.51-0600 [STG/0] OUT Successfully destroyed container
2017-08-17T08:48:20.24-0600 [CELL/0] OUT Successfully created container
2017-08-17T08:48:29.33-0600 [APP/PROC/WEB/0] OUT Running .\CloudFoundry
2017-08-17T08:48:29.79-0600 [APP/PROC/WEB/0] OUT Now listening on: http://0.0.0.0:56925
2017-08-17T08:48:29.79-0600 [APP/PROC/WEB/0] OUT Hosting environment: Development
2017-08-17T08:48:29.79-0600 [APP/PROC/WEB/0] OUT Content root path: C:\containerizer\B91BBA946E8B925107\user\app
2017-08-17T08:48:29.79-0600 [APP/PROC/WEB/0] OUT Application started. Press Ctrl+C to shut down.
```

Once the app is up and running then you can access the management endpoints exposed by Steeltoe using the [Pivotal Apps Manager](https://docs.pivotal.io/pivotalcf/1-11/console/index.html).

The Steeltoe Management framework exposes Spring Boot Actuator compatible Endpoints which can be used using the Pivotal Apps Manager. By using the Apps Manager, you can view the Apps Health, Build Information (e.g. Git info, etc), and recent Request/Response Traces, as well as manage/change the applications logging levels.

Check out the Apps Manager, [Using Spring Boot Actuators](https://docs.pivotal.io/pivotalcf/1-11/console/using-actuators.html) for more information.

---

### See the Official [Steeltoe Management Documentation](https://steeltoe.io/docs/steeltoe-management) for a more in-depth walkthrough of the samples and more detailed information