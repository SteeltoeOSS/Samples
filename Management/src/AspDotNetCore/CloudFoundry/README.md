# Management Sample App for Cloud Foundry

ASP.NET Core sample app illustrating how to use [Steeltoe Management Endpoints](https://github.com/SteeltoeOSS/Management) together with the [Pivotal Apps Manager](https://docs.pivotal.io/pivotalcf/1-11/console/index.html) for monitoring and managing your applications on Cloud Foundry.  

This application also illustrates how to have application metrics captured and exported to the [Metrics Forwarder for PCF](https://docs.pivotal.io/metrics-forwarder/index.html) service so that applications metrics can be viewed in any tool that is able to consume those metrics from the [Cloud Foundry Loggregator Firehose](https://docs.pivotal.io/pivotalcf/2-1/loggregator/architecture.html#firehose).  Several tools exist that can do this, including [PCF Metrics](https://docs.pivotal.io/pcf-metrics/1-4/index.html) from Pivotal.

## Pre-requisites - CloudFoundry

1. Installed Pivotal Cloud Foundry
2. Installed Apps Manager on Cloud Foundry
3. Installed MySql CloudFoundry service
4. Optionally install [Metrics Forwarder for PCF](https://network.pivotal.io/products/p-metrics-forwarder).
5. Optionally install [PCF Metrics](https://network.pivotal.io/products/apm).
6. Install .NET Core SDK

## Create MySql Service Instance on CloudFoundry

You must first create an instance of the MySql service in a org/space.

1. cf target -o myorg -s development
2. cf create-service p-mysql 100mb myMySqlService

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
2. cd samples/Management/src/AspDotNetCore/CloudFoundry
3. dotnet restore --configfile nuget.config
4. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`)
5. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`)

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

Once the app is up and running then you can access the management endpoints exposed by Steeltoe using the [Pivotal Apps Manager](https://docs.pivotal.io/pivotalcf/2-1/console/).

The Steeltoe Management framework exposes Spring Boot Actuator compatible Endpoints which can be used using the Pivotal Apps Manager. By using the Apps Manager, you can view the Apps Health, Build Information (e.g. Git info, etc), and recent Request/Response Traces, as well as manage/change the applications logging levels.

Check out the Apps Manager, [Using Spring Boot Actuators](https://docs.pivotal.io/pivotalcf/2-1/console/using-actuators.html) for more information.

## View Application Metrics in PCF Metrics

If you wish to collect and view applications metrics in [PCF Metrics](http://docs.pivotal.io/pcf-metrics/1-4/index.html), you first must bind an instance of the [Metrics Forwarder](https://docs.pivotal.io/metrics-forwarder/index.html) service to your application and restart it.  Once thats complete custom metrics will be collected and automatically exported to the Metrics Forwarder service.  

1. cf target -o myorg -s development
2. cf create-service metrics-forwarder unlimited my-metrics
3. cf bind-service actuator my-metrics
4. cf restart actuator

To view the metrics you can use the [PCF Metrics](https://network.pivotal.io/products/apm) tool from Pivotal. Follow the instructions in the [documentation](http://docs.pivotal.io/pcf-metrics/1-4/) and pay particular attention to the section on viewing [Custom Metrics](http://docs.pivotal.io/pcf-metrics/1-4/using.html).

---

### See the Official [Steeltoe Management Documentation](https://steeltoe.io/docs/steeltoe-management) for a more in-depth walk-through of the samples and more detailed information