# Fortune Teller UI - ASP.NET MVC Application

This ASP.NET MVC sample app illustrates how to use [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

> Note: This application is built using the Autofac IOC container.

## Running Local

### Pre-requisites

1. Microsoft Windows
1. Visual Studio 2017+

### Running a Eureka Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Eureka-Server) for detailed instructions on running a local Eureka server.

### Starting the App

This is a typical ASP.NET MVC project that runs on the 4.x line of .NET Framework. Use Visual Studio to build and run the service.
Once the Fortune Teller UI is up and running, browse to <http://localhost:5555/> to see it!  Be sure to have at least one FortuneService up and running first!

## Running on Cloud Foundry

### Setup Service Registry on Cloud Foundry

Using the service instance name of `myDiscoveryService`, complete the [common task](/CommonTasks.md#Spring-Cloud-Eureka-Server) of provisioning a Eureka server.

### Publish App & Push to Cloud Foundry

1. Open Samples\Discovery\src\AspDotNetAutofac\FortuneTeller.sln in Visual Studio.
1. Select Fortune-Teller-UI4 project in Solution Explorer.
1. Right-click and select Publish
1. Publish the using the provided `FolderProfile`
1. Open your preferred command prompt and `cd` into the `Fortune-Teller-UI4` folder
1. Run `cf push -p bin\Debug\net461\win10-x64\publish`

## What to expect - CloudFoundry

You should see something like this during startup:

```bash
2018-05-04T10:21:57.606-05:00 [CELL/0] [OUT] Creating container
2018-05-04T10:21:58.125-05:00 [CELL/0] [OUT] Successfully destroyed container
2018-05-04T10:21:58.725-05:00 [CELL/0] [OUT] Successfully created container
2018-05-04T10:22:01.108-05:00 [CELL/0] [OUT] Starting health monitoring of container
2018-05-04T10:22:02.303-05:00 [APP/PROC/WEB/0] [OUT] Server Started for dff521d0-8232-4b10-b884-65c549f8036f
2018-05-04T10:22:05.745-05:00 [CELL/0] [OUT] Container became healthy
```

At this point the Fortune Teller UI is up and running and ready to display your fortune. Browse to <https://fortuneui.x.y.z/> to see it!

---

### See the [Service Discovery](https://steeltoe.io/service-discovery) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
