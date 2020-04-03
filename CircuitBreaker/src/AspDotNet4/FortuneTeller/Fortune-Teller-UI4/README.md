# Fortune Teller UI - ASP.NET MVC Application

This ASP.NET MVC app demonstrates the usage of Steeltoe Circuit Breaker commands and fall-backs while acting as the front end for the [Fortune Teller Service](../Fortune-Teller-Service4). [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud) is used for dynamic discovery of back-end service instances.

This sample also illustrates how to use the [Hystrix Dashboard](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#_circuit_breaker_hystrix_dashboard) to gather status and metrics of the Hystrix command used in communications.

> Note: This application is built using the Autofac IOC container. Autofac is not required for using Steeltoe Circuit Breaker, but it does make the functionality easier to use.

## Running Local

### Pre-requisites

1. Microsoft Windows
1. Visual Studio 2017+

### Running a Eureka Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Eureka-Server) for detailed instructions on running a local Eureka server.

### Starting the App

This is a typical ASP.NET MVC project that runs on the 4.x line of .NET Framework. Use Visual Studio to build and run the service.
If the fortune service is not running (or unreachable), you can expect to always see the same fortune returned in the web UI.

### Running a Hystrix Dashboard

Refer to [common tasks](/CommonTasks.md#Hystrix-Dashboard) for detailed instructions on running a local Hystrix dashboard.

## Running on Cloud Foundry

### Setup Service Registry on Cloud Foundry

Using the service instance name of `myDiscoveryService`, complete the [common task](/CommonTasks.md#Spring-Cloud-Eureka-Server) of provisioning a Eureka server.

### Setup Circuit Breaker Dashboard service on Cloud Foundry

Using the service instance name of `myHystrixService`, complete the [common task](/CommonTasks.md#Hystrix-Dashboard) of provisioning a Hystrix dashboard.

### Publish App & Push to Cloud Foundry

1. Open Samples\CircuitBreaker\src\AspDotNet4\FortuneTeller.sln in Visual Studio.
1. Select Fortune-Teller-UI4 project in Solution Explorer.
1. Right-click and select Publish
1. Publish the using the provided `FolderProfile`
1. Open your preferred command prompt and `cd` into the `Fortune-Teller-UI4` folder
1. Run `cf push -p bin\Debug\net461\win10-x64\publish`

### What to expect - Cloud Foundry

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

### Using the Hystrix Dashboard - Cloud Foundry

Once you have the two applications communicating, you can make use of the Hystrix dashboard by following the instructions below.

1. Open a browser or browser window and connect to the Pivotal Apps Manager.  You will have to use a link that is specific to your Cloud Foundry setup. (e.g. <https://apps.system.testcloud.com>)
2. Follow [these instructions](https://docs.pivotal.io/spring-cloud-services/1-3/common/circuit-breaker/using-the-dashboard.html) to open the Hystrix dashboard service.
3. Go back to the Fortune-Teller-UI application and obtain several fortunes.  Observe the values changing in the Hystrix dashboard.  Click the refresh button on the UI app quickly to see the dashboard update.

---

### See the [Circuit Breaker](https://steeltoe.io/circuit-breakers) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
