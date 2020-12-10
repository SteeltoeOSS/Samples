# Fortune-Teller-UI - ASP.NET Core MVC Application

ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering micro services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

## Running Local

### Running a Eureka Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Eureka-Server) for detailed instructions on running a local Eureka server.

## Building & Running - Local

1. Clone this repository: `git clone https://github.com/SteeltoeOSS/Samples`
1. `cd samples/Discovery/src/Fortune-Teller/Fortune-Teller-UI`
1. `dotnet restore`
1. `dotnet run -f netcoreapp3.1`

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/Fortune-Teller-UI
$ dotnet run -f netcoreapp3.1
Hosting environment: Production
Now listening on: http://*:5555
Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <http://localhost:5555/> to see it!

## Running on Cloud Foundry

### Setup Service Registry on Cloud Foundry

Using the service instance name of `myDiscoveryService`, complete the [common task](/CommonTasks.md#Spring-Cloud-Eureka-Server) of provisioning a Eureka server.

## Publish App & Push to CloudFoundry

1. Login and target your desired space/org: `cf target -o myorg -s myspace`
1. `cd samples/Discovery/src/Fortune-Teller/Fortune-Teller-Service`
1. Publish the app, selecting the framework and runtime you want to run on:
   - `dotnet publish -f netcoreapp3.1 -r linux-x64`
1. Push the app using the appropriate manifest:
   - `cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish`
   - `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish`

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

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <https://fortuneui.x.y.z/> to see it!

## Enabling SSL usage on Cloud Foundry

If you wish to use SSL communications between the Fortune Teller UI and the Fortune Teller Service, have a look at the comments in the files listed below.  You will need to make modifications to one or more of those files. Also, you are encouraged to read the [Cloud Foundry documentation](https://docs.pivotal.io/pivotalcf/1-10/adminguide/securing-traffic.html) on how SSL is configured, used and implemented before trying to use it with the Fortune Teller app.

1. `FortuneService.cs` - Changes needed to enable SSL usage when the Fortune Teller Service (i.e. Kestrel) is presenting Self-Signed certificates to the client.

---

### See the [Service Discovery](https://steeltoe.io/service-discovery) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
