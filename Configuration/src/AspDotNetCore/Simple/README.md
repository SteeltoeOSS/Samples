# Simple - ASP.NET Core Sample Application

ASP.NET Core sample app illustrating how to use [Spring Cloud Config Server](https://projects.spring.io/spring-cloud) as a configuration source.

## Pre-requisites

This sample assumes that there is a running Spring Cloud Config Server on your machine. To make this happen:

1. Install Java 8 JDK.
1. Install Maven 3.x.
1. Clone the Spring Cloud Config Server repository. (<https://github.com/spring-cloud/spring-cloud-config>)
1. Go to the config server directory (`spring-cloud-config/spring-cloud-config-server`) and fire it up with `mvn spring-boot:run`
1. This sample will default to looking for its spring cloud config server on localhost, so it should all connect.

The default configuration of the Config Server uses [this github repo](https://github.com/spring-cloud-samples/config-repo) for its source of configuration data.

## Building & Running

1. Clone this repo. (e.g. `git clone https://github.com/SteeltoeOSS/Samples`)
1. cd samples/Configuration/src/AspDotNetCore/Simple
1. Install .NET Core SDK 2.0
1. dotnet restore --configfile nuget.config
1. dotnet run -f netcoreapp2.0 or dotnet run -f net461

## What to expect

After building and running the app, you should see something like the following:

```text
$ cd samples/Configuration/src/AspDotNetCore/Simple
$ dotnet run -f netcoreapp2.0
Hosting environment: Production
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

Fire up a browser and hit <http://localhost:5000>.  Once on the home page, navigate to the `Config Server Data` tab and you'll see the values stored in the github repository used for the Spring Cloud Config Server samples.

If you navigate to the "Config Server Settings" tab you will see the settings used by the Spring Cloud Config server client.

Change the Hosting environment setting to `development` (i.e. export ASPNETCORE_ENVIRONMENT=development), then restart the application. You will see different configuration data returned for that profile/hosting environment.

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/docs/steeltoe-configuration) for a more in-depth walkthrough of the samples and more detailed information