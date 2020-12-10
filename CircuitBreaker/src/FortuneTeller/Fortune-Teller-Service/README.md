# Fortune Teller Service - ASP.NET Core Microservice

This WebApi sample app is the backing service for the [Fortune Teller UI](./Fortune-Teller-UI). This service does register with [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud) so that it can be discovered by the Fortune Teller UI, but the primary focus of this sample is the circuit breaker commands and fallbacks that can be found in the UI application.

## Running Local

### Running a Eureka Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Eureka-Server) for detailed instructions.

### Building & Running - Local

1. Clone this repository. (`git clone https://github.com/SteeltoeOSS/Samples`)
1. `cd samples/CircuitBreaker/src/Fortune-Teller/Fortune-Teller-Service`
1. `dotnet run -f netcoreapp3.1`

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/CircuitBreaker/src/Fortune-Teller/Fortune-Teller-Service
$ dotnet run -f netcoreapp3.1
info: Microsoft.Data.Entity.Storage.Internal.InMemoryStore[1]
      Saved 50 entities to in-memory store.
Hosting environment: Production
Now listening on: http://*:5000
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller Service is up and waiting for the Fortune Teller UI to ask for fortunes.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Installed Spring Cloud Services
1. Install .Net Core SDK

### Setup Service Registry on CloudFoundry

Using the service instance name of `myDiscoveryService`, complete the [common task](/CommonTasks.md#Spring-Cloud-Eureka-Server) of provisioning a Eureka server.

## Publish App & Push to CloudFoundry

1. Login and target your desired space/org: `cf target -o myorg -s myspace`
1. `cd samples/CircuitBreaker/src/Fortune-Teller/Fortune-Teller-Service`
1. Publish the app, selecting the framework and runtime you want to run on:
   - `dotnet publish -f netcoreapp3.1 -r linux-x64`
1. Push the app using the appropriate manifest:
   - `cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish`
   - `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish`

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneservice`

On a Windows cell, you should see something like this during startup:

```bash
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

---

### See the [Circuit Breaker](https://steeltoe.io/circuit-breakers) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
