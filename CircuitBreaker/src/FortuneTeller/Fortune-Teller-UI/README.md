# Fortune-Teller-UI - ASP.NET Core MVC Application

ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud) for registering micro services and [Spring Cloud Hystrix](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#_circuit_breaker_hystrix_clients) for building resilient micro services applications. The Fortune-Teller-Service registers the fortuneService with the Eureka server upon startup and the Fortune-Teller-UI uses a Hystrix Command with fallback ability when communicating with the Fortune service.

In addition, the Fortune-Teller-UI also illustrates how to use a Hystrix Collapser to combine, or 'batch up', multiple requests to backend micro-services endpoints.

This sample also illustrates how to use the [Hystrix Dashboard](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#_circuit_breaker_hystrix_dashboard) to gather status and metrics of the Hystrix command used in communications.

## Running Local

### Pre-requisites - Local

This sample assumes that there is a running Spring Cloud Eureka Server on your machine and a Hystrix dashboard. To make this happen:

1. Refer to common steps for [running a local Eureka server](/CommonTasks.md#Spring-Cloud-Eureka-Server)
1. Refer to common steps for [running a local Hystrix dashboard](/CommonTasks.md#Hystrix-Dashboard)
1. Install .NET Core SDK

### Building & Running - Local

1. Clone this repository: `git clone https://github.com/SteeltoeOSS/Samples`
1. `cd samples/CircuitBreaker/src/Fortune-Teller/Fortune-Teller-UI`
1. Set the `BUILD` environment variable to `LOCAL` (i.e. SET BUILD=LOCAL, export BUILD=LOCAL)
1. Set PORT to listen on (i.e SET PORT=5555, export PORT=5555)
1. `dotnet restore`
1. `dotnet run -f netcoreapp3.1`

### What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/CircuitBreaker/src/Fortune-Teller/Fortune-Teller-UI
$ dotnet run -f netcoreapp3.1
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://localhost:5555/> to see it!

### Using the Hystrix Dashboard - Local

Once you have the two applications communicating, you use of the Hystrix dashboard by following the instructions below.  Note: This assumes you have followed the steps above and already have the dashboard running.

1. Open a browser or browser window and connect to the dashboard. (e.g. <http://localhost:7979>)
1. In the first field, enter the endpoint (<http://localhost:5555/hystrix/hystrix.stream>) that is exposing the hystrix metrics.
1. Click the monitor button.
1. Go back to the Fortune-Teller-UI application and obtain several fortunes.  Observe the values changing in the Hystrix dashboard.  Click the refresh button on the UI app quickly to see the dashboard update.

## Running on Cloud Foundry

### Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Installed Spring Cloud Services
1. Install .NET Core SDK

### Setup Service Registry on CloudFoundry

Using the service instance name of `myDiscoveryService`, complete the [common task](/CommonTasks.md#Spring-Cloud-Eureka-Server) of provisioning a Eureka server.

### Setup Circuit Breaker Dashboard service on CloudFoundry

Using the service instance name of `myHystrixService`, complete the [common task](/CommonTasks.md#Hystrix-Dashboard) of provisioning a Hystrix dashboard.

### Publish App & Push to CloudFoundry

1. `cf target -o myorg -s development`
1. cd samples/CircuitBreaker/src/Fortune-Teller/Fortune-Teller-UI
1. Make sure environment variable `BUILD` is not set to `LOCAL` (i.e. SET BUILD=, unset BUILD)
1. `dotnet restore`
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp3.1 -r linux-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish`)

### What to expect - CloudFoundry

You should see something like this during startup:

```bash
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

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <https://fortuneui.x.y.z/> to see it!

In addition to hitting <https://fortuneui.x.y.z/>, you can also hit: <https://fortuneui.x.y.z/#/multiple> to cause the UI to make use of a Hystrix Collapser to obtain multiple fortunes.

### Using the Hystrix Dashboard - Cloud Foundry

Once you have the two applications communicating, you can make use of the Hystrix dashboard by following the instructions below.

1. Open a browser or browser window and connect to the Pivotal Apps Manager.  You will have to use a link that is specific to your Cloud Foundry setup. (e.g. <https://apps.system.testcloud.com>)
1. Follow [these instructions](https://docs.pivotal.io/spring-cloud-services/1-3/common/circuit-breaker/using-the-dashboard.html) to open the Hystrix dashboard service.
1. Go back to the Fortune-Teller-UI application and obtain several fortunes.  Observe the values changing in the Hystrix dashboard.  Click the refresh button on the UI app quickly to see the dashboard update.

---

### See the [Circuit Breaker](https://steeltoe.io/circuit-breakers) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
