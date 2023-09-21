# Fortune-Teller-UI - ASP.NET Core MVC Application

ASP.NET Core sample app illustrating how to use [Spring Cloud Eureka Server](https://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#spring-cloud-eureka-server) for discovering micro services. The Fortune-Teller-UI attempts to locate the fortuneService in the Eureka server and uses it to get your fortune.

## Running Local

### Running a Eureka Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Eureka-Server) for detailed instructions on running a local Eureka server.

## Building & Running - Local

1. Clone this repository: `git clone https://github.com/SteeltoeOSS/Samples`
1. `cd samples/Discovery/src/Fortune-Teller/Fortune-Teller-UI`
1. `dotnet restore`
1. `dotnet run`

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
$ cd samples/Discovery/src/Fortune-Teller-UI
$ dotnet run
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
   - `dotnet publish -r linux-x64 --self-contained`
   - `dotnet publish -r win-x64 --self-contained`
1. Push the app using the appropriate manifest:
   - `cf push -f manifest.yml -p bin/Debug/net6.0/linux-x64/publish`
   - `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`

## What to expect - CloudFoundry

After building and running the app, you should see something like the following in the logs.

To see the logs as you startup and use the app: `cf logs fortuneui`

On a Windows cell, you should see something like this during startup:

```bash
   2023-09-19T17:48:15.92+0200 [CELL/0] OUT Starting health monitoring of container
   2023-09-19T17:48:19.47+0200 [APP/PROC/WEB/0] OUT info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[63]
   2023-09-19T17:48:19.47+0200 [APP/PROC/WEB/0] OUT User profile is available. Using 'C:\Users\vcap\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
   2023-09-19T17:48:21.08+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[100]
   2023-09-19T17:48:21.08+0200 [APP/PROC/WEB/0] OUT Start processing HTTP request GET https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/
   2023-09-19T17:48:21.08+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[100]
   2023-09-19T17:48:21.08+0200 [APP/PROC/WEB/0] OUT Sending HTTP request GET https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/
   2023-09-19T17:48:21.13+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[101]
   2023-09-19T17:48:21.13+0200 [APP/PROC/WEB/0] OUT Received HTTP response headers after 81.4796ms - 200
   2023-09-19T17:48:21.13+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[101]
   2023-09-19T17:48:21.13+0200 [APP/PROC/WEB/0] OUT End processing HTTP request after 103.9759ms - 200
   2023-09-19T17:48:21.40+0200 [APP/PROC/WEB/0] OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
   2023-09-19T17:48:21.40+0200 [APP/PROC/WEB/0] OUT DoGetApplicationsAsync https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/, status: OK, applications: Applications[Application[Name=FORTUNESERVICE,Instances=Instance[InstanceId=fortuneService.apps.clovis.cf-app.com:475e5c88-449b-489e-4c01-a498,HostName=fortuneService.apps.clovis.cf-app.com,IpAddr=10.255.95.97,Status=Up,IsInsecurePortEnabled=True,Port=80,IsSecurePortEnabled=False,SecurePort=443,VipAddress=fortuneService,SecureVipAddress=fortuneService,ActionType=Added],]], retry: 0
   2023-09-19T17:48:21.40+0200 [APP/PROC/WEB/0] OUT info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[58]
   2023-09-19T17:48:21.40+0200 [APP/PROC/WEB/0] OUT Creating key {7e38e8c9-e495-46f6-bc92-07d70adb1400} with creation date 2023-09-19 15:48:21Z, activation date 2023-09-19 15:48:21Z, and expiration date 2023-12-18 15:48:21Z.
   2023-09-19T17:48:21.40+0200 [APP/PROC/WEB/0] OUT info: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[39]
   2023-09-19T17:48:21.40+0200 [APP/PROC/WEB/0] OUT Writing data to file 'C:\Users\vcap\AppData\Local\ASP.NET\DataProtection-Keys\key-7e38e8c9-e495-46f6-bc92-07d70adb1400.xml'.
   2023-09-19T17:48:21.74+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[100]
   2023-09-19T17:48:21.74+0200 [APP/PROC/WEB/0] OUT Start processing HTTP request GET https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/
   2023-09-19T17:48:21.74+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[100]
   2023-09-19T17:48:21.74+0200 [APP/PROC/WEB/0] OUT Sending HTTP request GET https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/
   2023-09-19T17:48:21.77+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.ClientHandler[101]
   2023-09-19T17:48:21.77+0200 [APP/PROC/WEB/0] OUT Received HTTP response headers after 15.1045ms - 200
   2023-09-19T17:48:21.77+0200 [APP/PROC/WEB/0] OUT info: System.Net.Http.HttpClient.Eureka.LogicalHandler[101]
   2023-09-19T17:48:21.77+0200 [APP/PROC/WEB/0] OUT End processing HTTP request after 15.3277ms - 200
   2023-09-19T17:48:21.77+0200 [APP/PROC/WEB/0] OUT dbug: Steeltoe.Discovery.Eureka.Transport.EurekaHttpClient[0]
   2023-09-19T17:48:21.77+0200 [APP/PROC/WEB/0] OUT DoGetApplicationsAsync https://service-registry-4124f73a-5077-42fd-9aa2-a9ffb51fd879.apps.clovis.cf-app.com/eureka/apps/, status: OK, applications: Applications[Application[Name=FORTUNESERVICE,Instances=Instance[InstanceId=fortuneService.apps.clovis.cf-app.com:475e5c88-449b-489e-4c01-a498,HostName=fortuneService.apps.clovis.cf-app.com,IpAddr=10.255.95.97,Status=Up,IsInsecurePortEnabled=True,Port=80,IsSecurePortEnabled=False,SecurePort=443,VipAddress=fortuneService,SecureVipAddress=fortuneService,ActionType=Added],]], retry: 0
   2023-09-19T17:48:21.83+0200 [APP/PROC/WEB/0] OUT info: Microsoft.Hosting.Lifetime[14]
   2023-09-19T17:48:21.83+0200 [APP/PROC/WEB/0] OUT Now listening on: http://0.0.0.0:8080
   2023-09-19T17:48:21.83+0200 [APP/PROC/WEB/0] OUT info: Steeltoe.Management.Diagnostics.DiagnosticObserver[0]
   2023-09-19T17:48:21.83+0200 [APP/PROC/WEB/0] OUT DiagnosticObserver TraceDiagnosticObserver Subscribed to Microsoft.AspNetCore
   2023-09-19T17:48:21.83+0200 [APP/PROC/WEB/0] OUT info: Steeltoe.Management.Diagnostics.DiagnosticObserver[0]
   2023-09-19T17:48:21.83+0200 [APP/PROC/WEB/0] OUT DiagnosticObserver AspNetCoreHostingObserver Subscribed to Microsoft.AspNetCore
   2023-09-19T17:48:21.84+0200 [APP/PROC/WEB/0] OUT info: Microsoft.Hosting.Lifetime[0]
   2023-09-19T17:48:21.84+0200 [APP/PROC/WEB/0] OUT Application started. Press Ctrl+C to shut down.
```

At this point the Fortune Teller UI is up and running and ready for displaying your fortune. Hit <https://fortuneui.x.y.z/> to see it!

## Enabling SSL usage on Cloud Foundry

If you wish to use SSL communications between the Fortune Teller UI and the Fortune Teller Service, have a look at the comments in the files listed below.  You will need to make modifications to one or more of those files. Also, you are encouraged to read the [Cloud Foundry documentation](https://docs.pivotal.io/pivotalcf/1-10/adminguide/securing-traffic.html) on how SSL is configured, used and implemented before trying to use it with the Fortune Teller app.

1. `FortuneService.cs` - Changes needed to enable SSL usage when the Fortune Teller Service (i.e. Kestrel) is presenting Self-Signed certificates to the client.

---

### See the [Service Discovery](https://steeltoe.io/service-discovery) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
