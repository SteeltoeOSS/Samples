# SimpleCloudFoundry - ASP.NET Core Sample Application

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BConfiguration_SimpleCloudFoundry%5D?branchName=2.x)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=13&branchName=master)

ASP.NET Core sample app illustrating how to use [Config Server for Pivotal Cloud Foundry](https://docs.pivotal.io/spring-cloud-services/config-server/) as a configuration source.

## Pre-requisites

1. Installed Pivotal CloudFoundry
1. Installed Spring Cloud Services
1. .NET Core SDK

## Setup Config Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Config-Server) for detailed instructions on creating a Spring Cloud Config Server instance in Cloud Foundry. This sample expects the config server to be backed by the `spring-cloud-samples` repo.

## Publish App & Push

1. `cf target -o myorg -s development`
1. `cd src/SimpleCloudFoundry`
1. `dotnet restore`
1. Publish app to a directory selecting the framework and runtime you want to run on:
    - `dotnet publish -f netcoreapp3.1 -r linux-x64`
1. Push the app using the appropriate manifest:
    - `cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish`
    - `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish`

## What to expect

The `cf push` will create an app in the space by the name `foo` and will bind the `myConfigServer` service instance to the app. You can hit the app @ `https://foo.x.y.z/`.

The Config Server should be backed by this Git repository: `https://github.com/spring-cloud-samples/config-repo`

Use the menus at the top of the app to see various output:

- `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.
- `Config Server Settings` - should show the settings used by the client when communicating to the config server.  These have been picked up from the service binding.
- `Config Server Data` - this is the configuration data returned from the Config Servers Git repository. It will be some of the data from `foo.properties`, `foo-development.properties` and `application.yml` found in the Git repository.
- `Reload` - will cause a reload of the configuration data from the server.

---

### See the [App Configuration](https://steeltoe.io/app-configuration) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
