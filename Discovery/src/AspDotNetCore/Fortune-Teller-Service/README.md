# Fortune-Teller-Service - ASP.NET Core Microservice

ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for registering micro services. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup.

Note: You can run this either locally or on CloudFoundry.

## Pre-requisites - Running Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine. To make this happen:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Samples Eureka repository. <https://github.com/spring-cloud-samples/eureka.git>
1. Go to the eureka server directory (`eureka`) and fire it up with `mvn spring-boot:run`
1. When running locally, this sample will default to looking for its eurka server on <http://localhost:8761/eureka>, so it should all connect.
1. Install .NET Core SDK

## Building & Running - Local

1. Clone this repo. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
1. dotnet restore --configfile nuget.config
1. dotnet run -f netcoreapp2.0

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
$ dotnet run -f netcoreapp2.0
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.

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
1. cd samples/Discovery/src/AspDotNetCore/Fortune-Teller-Service
1. dotnet restore --configfile nuget.config
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish  -f netcoreapp2.0 -r ubuntu.14.04-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish`)

> Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing this app. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Discovery Client.  You can do this by editing `appsettings.json` and add `eureka:client:validate_certificates=false`.

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs. 

To see the logs as you startup and use the app: `cf logs fortuneservice`

On a Windows cell, you should see something like this during startup:

```bash
2016-05-14T06:22:39.54-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:22:39.54-0600 [APP/0]      OUT       GetRequestContent generated JSON: ......
2016-05-14T06:22:39.57-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:22:39.58-0600 [APP/0]      OUT       RegisterAsync .....
2016-05-14T06:22:39.58-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:22:39.58-0600 [APP/0]      OUT       Register FORTUNESERVICE/fortuneService.apps.testcloud.com:2f7a9e48-bb3e-402a-6b44-68e9386b3b15 returned: NoContent
2016-05-14T06:22:41.07-0600 [APP/0]      OUT info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
2016-05-14T06:22:41.07-0600 [APP/0]      OUT       Saved 50 entities to in-memory store.
2016-05-14T06:22:41.17-0600 [APP/0]      OUT       Hosting starting
2016-05-14T06:22:41.17-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-14T06:22:41.19-0600 [APP/0]      OUT       Start
2016-05-14T06:22:41.19-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:22:41.23-0600 [APP/0]      OUT       Listening on prefix: http://*:57991/
2016-05-14T06:22:41.23-0600 [APP/0]      OUT info: Microsoft.Net.Http.Server.WebListener[0]
2016-05-14T06:22:41.31-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-14T06:22:41.31-0600 [APP/0]      OUT       Hosting started
2016-05-14T06:22:41.31-0600 [APP/0]      OUT Hosting environment: development
2016-05-14T06:22:41.32-0600 [APP/0]      OUT Now listening on: http://*:57991
2016-05-14T06:22:41.32-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-05-14T06:23:09.76-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
2016-05-14T06:23:09.76-0600 [APP/0]      OUT       SendHeartbeatAsync ......., status: OK, instanceInfo: null
2016-05-14T06:23:09.76-0600 [APP/0]      OUT dbug: Steeltoe.Discovery.Eureka.DiscoveryClient[0]
2016-05-14T06:23:09.76-0600 [APP/0]      OUT       Renew FORTUNESERVICE/fortuneService.apps.testcloud.com:2f7a9e48-bb3e-402a-6b44-68e9386b3b15 returned: OK
```

At this point the Fortune Teller Service is up and running and ready for the Fortune Teller UI to ask for fortunes.

## Using Container-to-Container on Cloud Foundry

If you wish to use Container-to-Container (C2C) communications between the UI and the Fortune Service, look at the comments in the files listed below.  You will need to make modifications to those files.  Also, you are encouraged to read the Cloud Foundry [C2C documentation](https://docs.pivotal.io/pivotalcf/1-10/devguide/deploy-apps/cf-networking.html) FIRST, before trying to use it with the Fortune Teller app.

1. `appsettings.json` - Eureka registration option settings

## Enabling SSL usage on Cloud Foundry

If you wish to use SSL communications between the Fortune Teller UI and the Fortune Teller Service, have a look at the comments in the files listed below.  You will need to make modifications to one or more of those files. Also, you are encouraged to read the [Cloud Foundry documentation](https://docs.pivotal.io/pivotalcf/1-10/adminguide/securing-traffic.html) on how SSL is configured, used and implemented before trying to use it with the Fortune Teller app.

1. `appsettings.json` - Eureka registration option settings
2. `Program.cs` - Changes needed to enable SSL usage with Kestrel (Note: These changes are only required if using Containter-to-Container (C2C) networking, together with SSL, between the UI and the fortune service).
3. `manifest.yml` - Startup command line change (Note: This change is only required if using Containter-to-Container (C2C) networking, together with SSL, between the UI and the fortune service).

---

### See the Official [Steeltoe Service Discovery Documentation](https://steeltoe.io/docs/steeltoe-service-discovery) for a more in-depth walkthrough of the samples and more detailed information