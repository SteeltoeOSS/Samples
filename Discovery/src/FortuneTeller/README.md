# FortuneTeller Discovery Sample Application

This sample demonstrates Steeltoe Service Discovery. It consists of the following projects:

* FortuneTellerService - ASP.NET microservice illustrating how to register with Consul or Eureka at startup.
* FortuneTellerWeb - ASP.NET MVC app illustrating how to consume registered services using Consul, Eureka or Configuration.

FortuneTellerService provides an API endpoint that returns a random fortune. FortuneTellerWeb accesses the API endpoint
at `https://fortuneService/api/fortunes/random`. The `https://fortuneService` part gets resolved using service discovery,
switching to `http` or `https` and replacing `fortuneService` with the real host name or IP and port number.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: Cloud Foundry-based platform such as Tanzu Application Service (optionally with Windows support)

## Configuration-based discovery

This variant uses service instances that are registered in the .NET configuration of FortuneTellerWeb.

> [!NOTE]
> For the purpose of running this sample in various modes, the entries are defined in `launchSettings.json`.
> Normally you would define them in `appsettings.json`, for example:
> ```json
> {
>   "Discovery": {
>     "Services": [
>       {
>         "ServiceId": "fortuneService",
>         "Host": "localhost",
>         "Port": "7251",
>         "IsSecure": true
>       },
>       {
>         "ServiceId": "fortuneService",
>         "Host": "localhost",
>         "Port": "5160",
>         "IsSecure": false
>       }
>     ]
>   }
> }
> ```

1. Start FortuneTellerService with the **None** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "None"
   ```
1. Navigate to the [fortune endpoint](https://localhost:7251/api/fortunes/random) in your browser and observe a random fortune
1. Start FortuneTellerWeb with the **Configuration** launch profile
   ```
   cd FortuneTellerWeb
   dotnet run --launch-profile "Configuration"
   ```
1. Navigate to the [app](https://localhost:7233) in your browser and observe a random fortune

## Consul

This variant uses service instances that are registered in [HashiCorp Consul](https://www.consul.io/).
To determine liveliness, FortuneTellerService periodically sends TTL heartbeats to Consul.

### Running locally

1. Start a Consul [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Start FortuneTellerService with the **Consul** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "Consul"
   ```
1. Navigate to the [fortune endpoint](https://localhost:7251/api/fortunes/random) in your browser and observe a random fortune
1. Navigate to the [Consul dashboard](http://localhost:8500) in your browser and observe "fortuneService" is registered
   - Detailed information about the registrations can be viewed using the [Services API](http://localhost:8500/v1/agent/services) and [Health API](http://localhost:8500/v1/agent/checks)
1. Start FortuneTellerWeb with the **Consul** launch profile
   ```
   cd FortuneTellerWeb
   dotnet run --launch-profile "Consul"
   ```
1. Navigate to the [app](https://localhost:7233) in your browser and observe a random fortune

## Consul with health actuator

Instead of sending TTL heartbeats, the Steeltoe health actuator endpoint (`/actuator/health`) of FortuneTellerService
is periodically queried by Consul to determine liveliness.

### Running locally

1. Start a Consul [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Start FortuneTellerService with the **ConsulActuator** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "ConsulActuator"
   ```
1. Navigate to the [fortune endpoint](https://localhost:7251/api/fortunes/random) in your browser and observe a random fortune
1. Navigate to the [Consul dashboard](http://localhost:8500) in your browser and observe "fortuneService" is registered
   - Detailed information about the registrations can be viewed using the [Services API](http://localhost:8500/v1/agent/services) and [Health API](http://localhost:8500/v1/agent/checks)
1. Wait a few seconds, refreshing the Health API, and observe **Status** `passing` with **Type** `http`
1. In `Program.cs`, change the line with `ExampleHealthContributor` to report `HealthStatus.Down`
1. Restart FortuneTellerService with the **ConsulActuator** launch profile
1. Wait a few seconds, refreshing the Health API, and observe **Status** `critical` with **Type** `http`

## Eureka

This variant uses service instances that are registered in [Spring Cloud Eureka](https://cloud.spring.io/spring-cloud-netflix/reference/html/).

> [!NOTE]
> The Steeltoe Eureka docker image and the configuration settings used in this sample are tweaked to respond quickly to changes,
> at the cost of performance and scalability. Normally it takes several minutes for changes to become visible in Eureka.
> Don't use these tweaks for production scenarios.

### Running locally

1. Start a Eureka [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Start FortuneTellerService with the **Eureka** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "Eureka"
   ```
1. Navigate to the [fortune endpoint](https://localhost:7251/api/fortunes/random) in your browser and observe a random fortune
1. Navigate to the [Eureka dashboard](http://localhost:8761/) in your browser and observe "FORTUNESERVICE" is registered
   - Detailed information about the registrations can be viewed using the [Eureka API](http://localhost:8761/eureka/apps)
1. Start FortuneTellerWeb with the **Eureka** launch profile
   ```
   cd FortuneTellerWeb
   dotnet run --launch-profile "Eureka"
   ```
1. Navigate to the [app](https://localhost:7233) in your browser and observe a random fortune

### Running on CloudFoundry (Go-router)

1. Create a Eureka service instance in an org/space:
   ```
   cf target -o your-org -s your-space
   cf create-service p.service-registry standard myDiscoveryService
   ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command from the FortuneTellerService directory, wait until it has started, then run it from the FortuneTellerWeb directory
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

### Running on CloudFoundry (Container-to-Container)

Same steps as above, but uncomment the related line in `manifest.yaml` before push. Additionally:
1. In **Apps Manager**, go to **fortuneWeb** in your org/space
1. Go to **Container Networking** in the left-side menu
1. Click **CREATE POLICY**
1. Select the **fortuneService** app and specify **port** `8080`

For more information, read the Cloud Foundry documentation on [container-to-container](https://docs.cloudfoundry.org/devguide/deploy-apps/cf-networking.html) and [custom ports](https://docs.cloudfoundry.org/devguide/custom-ports.html).

## Eureka with health actuator

Aside from sending heartbeats, Steeltoe queries ASP.NET health checks and `IHealthContributor`s
(basically what runs behind the health actuator endpoint `/actuator/health`) of FortuneTellerService
to determine the local instance status during registration and renewals.

### Running locally

1. Start a Eureka [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Start FortuneTellerService with the **EurekaActuator** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "EurekaActuator"
   ```
1. Navigate to the [fortune endpoint](https://localhost:7251/api/fortunes/random) in your browser and observe a random fortune
1. Navigate to the [Eureka dashboard](http://localhost:8761/) in your browser and observe "FORTUNESERVICE" is registered
   - Detailed information about the registrations can be viewed using the [Eureka API](http://localhost:8761/eureka/apps)
1. Wait a few seconds, refreshing the Eureka API, and observe **status** `UP`
1. In `Program.cs`, change the line with `ExampleHealthContributor` to report `HealthStatus.Down`
1. Restart FortuneTellerService with the **EurekaActuator** launch profile
1. Wait a few seconds, refreshing the Eureka API, and observe **status** `DOWN`

## Eureka with dynamic port bindings

### Running locally

This variant registers FortuneTellerService with Eureka as before, but configures ASP.NET to dynamically assign ports.
Because port assignment occurs very late in the startup phase (after Eureka registration),
Steeltoe adds a random number to the Instance ID during registration to avoid collisions.

1. Start a Eureka [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Start FortuneTellerService with the **EurekaDynamicPorts** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "EurekaDynamicPorts"
   ```
1. Navigate to the [Eureka API](http://localhost:8761/eureka/apps) in your browser to determine the assigned port numbers.
   - Observe the Instance ID ends with a high number
   - Copy the port number from `securePort` for use below

1. Navigate to the fortune endpoint at `https://localhost:XXXXX/api/fortunes/random` (replace XXXXX with the port number) in your browser and observe a random fortune
1. Start FortuneTellerWeb with the **Eureka** launch profile
   ```
   cd FortuneTellerWeb
   dotnet run --launch-profile "Eureka"
   ```
1. Navigate to the [app](https://localhost:7233) in your browser and observe a random fortune

## Eureka with Config Server Discovery-First

This variant uses [Config Server](https://docs.spring.io/spring-cloud-config/docs/current/reference/html/) that registers itself
with Eureka at startup. FortuneTellerService is configured to obtain the URL to Config Server from Eureka.

### Running locally

1. Start a Eureka [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md) (pick the one for discovery-first)
1. Start a Config Server [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Start FortuneTellerService with the **Eureka** launch profile
   ```
   cd FortuneTellerService
   dotnet run --launch-profile "Eureka"
   ```
1. Navigate to the [Eureka dashboard](http://localhost:8761/) in your browser and observe both "FORTUNESERVICE" and "CONFIGSERVER" are registered
   - Detailed information about the registrations can be viewed using the [Eureka API](http://localhost:8761/eureka/apps)
1. Navigate to the [configuration endpoint](https://localhost:7251/api/configuration) in your browser and observe the configuration source is `http://host.docker.internal:8888/`. Without using discovery-first, the configuration source would have been `http://localhost:8888`.

---

### See the Official [Steeltoe Service Discovery Documentation](https://docs.steeltoe.io/api/v3/discovery) and [Steeltoe Getting Started with Discovery Guide](https://docs.steeltoe.io/guides/service-discovery/eureka.html) for a more in-depth walkthrough of the samples and more detailed information.
