# Fortune-Teller-UI - ASP.NET Core MVC Application

ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering micro services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

## Pre-requisites - Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Samples Eureka repository. (<https://github.com/spring-cloud-samples/eureka.git>)
1. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
1. This sample will default to looking for its eurka server on <http://localhost:8761/eureka>, so it should all connect.
1. Install .NET Core SDK

## Building & Running - Local

1. Clone this repo. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
1. dotnet restore --configfile nuget.config
1. dotnet run -f netcoreapp2.1

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
$ dotnet run -f netcoreapp2.1
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://localhost:5555/> to see it!

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Optionally install Windows support
1. Installed Spring Cloud Services
1. Install .NET Core SDK

## Setup Service Registry on CloudFoundry

You must first create an instance of the Service Registry service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-service-registry standard myDiscoveryService
1. Wait for the service to become ready! (i.e. cf services)

## Publish App & Push to CloudFoundry

1. cf target -o myorg -s development
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-UI
1. dotnet restore --configfile nuget.config
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`)

> Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing this app. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `eureka:client:validateCertificates=false`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneui`

On a Windows cell, you should see something like this during startup:

```bash
2016-05-14T06:38:21.67-0600 [CELL/0]     OUT Successfully created container
2016-05-14T06:38:27.78-0600 [APP/0]      OUT Running cmd /c SET "DNX_PACKAGES=%CD%\approot\packages" & approot\web.cmd
2016-05-14T06:38:47.90-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:38:47.90-0600 [APP/0]      OUT       DoGetApplicationsAsync .....
2016-05-14T06:38:47.91-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:38:47.91-0600 [APP/0]      OUT       FetchFullRegistry returned: OK, Applications[Application[Name=FORTUNESERVICE ....
2016-05-14T06:38:47.91-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:38:47.91-0600 [APP/0]      OUT       FetchRegistry succeeded
2016-05-14T06:38:47.99-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-14T06:38:47.99-0600 [APP/0]      OUT       Hosting starting
2016-05-14T06:38:48.02-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:38:48.02-0600 [APP/0]      OUT       Start
2016-05-14T06:38:48.07-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:38:48.07-0600 [APP/0]      OUT       Listening on prefix: http://*:58442/
2016-05-14T06:38:48.12-0600 [APP/0]      OUT       Hosting started
2016-05-14T06:38:48.12-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Hosting environment: development
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Now listening on: http://*:58442
2016-05-14T06:38:48.12-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://fortuneui.x.y.z/> to see it!

## Enabling SSL usage on Cloud Foundry

If you wish to use SSL communications between the Fortune Teller UI and the Fortune Teller Service, have a look at the comments in the files listed below.  You will need to make modifications to one or more of those files. Also, you are encouraged to read the [Cloud Foundry documentation](https://docs.pivotal.io/pivotalcf/1-10/adminguide/securing-traffic.html) on how SSL is configured, used and implemented before trying to use it with the Fortune Teller app.

1. `FortuneService.cs` - Changes needed to enable SSL usage when the Fortune Teller Service (i.e. Kestrel) is presenting Self-Signed certificates to the client.

---

### See the Official [Steeltoe Service Discovery Documentation](https://steeltoe.io/docs/steeltoe-service-discovery) for a more in-depth walkthrough of the samples and more detailed information