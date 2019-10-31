# Fortune Teller Service - ASP.NET WebApi Service

This WebApi sample app is the backing service for the [Fortune Teller UI](./Fortune-Teller-UI4). This service does register with [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud) so that it can be discovered by the Fortune Teller UI, but the primary focus of this sample is the circuit breaker commands and fallbacks that can be found in the UI application.

> Note: This application is built using the Autofac IOC container. Autofac is not required for using Steeltoe Circuit Breaker, but it does make the functionality easier to use.

## Running Local

### Pre-requisites

1. Microsoft Windows
1. Visual Studio 2017+

### Running a Eureka Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Eureka-Server) for detailed instructions.

### Starting the Service

This is a typical ASP.NET WebAPI project that runs on the 4.x line of .NET Framework. Use Visual Studio to build and run the service.

## Running on Cloud Foundry

### Setup Service Registry on Cloud Foundry

Using the service instance name of `myDiscoveryService`, complete the [common task](/CommonTasks.md#Spring-Cloud-Eureka-Server) of provisioning a Eureka server.

### Publish App & Push to CloudFoundry

1. Open Samples\CircuitBreaker\src\AspDotNet4\FortuneTeller.sln in Visual Studio.
1. Select Fortune-Teller-Service4 project in Solution Explorer.
1. Right-click and select Publish
1. Publish the using the provided `FolderProfile`
1. Open your preferred command prompt and `cd` into the `Fortune-Teller-Service4` folder
1. Run `cf push -p bin\Debug\net461\win10-x64\publish`

### What to expect - CloudFoundry

You should see something like this during startup:

```bash
2018-05-04T10:21:57.606-05:00 [CELL/0] [OUT] Creating container
2018-05-04T10:21:58.125-05:00 [CELL/0] [OUT] Successfully destroyed container
2018-05-04T10:21:58.725-05:00 [CELL/0] [OUT] Successfully created container
2018-05-04T10:22:01.108-05:00 [CELL/0] [OUT] Starting health monitoring of container
2018-05-04T10:22:02.303-05:00 [APP/PROC/WEB/0] [OUT] Server Started for dff521d0-8232-4b10-b884-65c549f8036f
2018-05-04T10:22:05.745-05:00 [CELL/0] [OUT] Container became healthy
```

At this point the Fortune Teller Service is up and running and ready for the [Fortune Teller UI](../Fortune-Teller-UI4/README.md) to ask for fortunes.

---

### See the [Circuit Breaker](https://steeltoe.io/circuit-breakers) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
