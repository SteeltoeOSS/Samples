# AutofacCloudFoundry - ASP.NET MVC Sample Application

ASP.NET MVC sample app built using the Autofac IOC container and illustrating how to use [Config Server for Pivotal Cloud Foundry](https://docs.pivotal.io/spring-cloud-services/config-server/) as a configuration source.

## Pre-requisites

1. Installed Pivotal CloudFoundry with Windows support
1. Installed Spring Cloud Services
1. Visual Studio 2017+

## Setup Config Server

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Config-Server) for detailed instructions on creating a Spring Cloud Config Server instance in Cloud Foundry. This sample expects the config server to be backed by the `spring-cloud-samples` repo.

## Publish App & Push

1. Open src\AspDotNet4\Configuration.sln in Visual Studio.
1. Select AutofacCloudFoundry project in Solution Explorer.
1. Right-click and select Publish
1. Publish the using the provided `FolderProfile`
1. Open your preferred command prompt and `cd` into the `AutofacCloudFoundry` folder
1. Run `cf push -p bin\Debug\net461\win10-x64\publish`

## What to expect

The cf push will create an app in the space by the name `foo` and will bind the `myConfigServer` service instance to the app. You can hit the app @ `https://foo.x.y.z/`.

The Config Server should be backed by this Git repository: `https://github.com/spring-cloud-samples/config-repo`

Use the menus at the top of the app to see various output:

* `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.
* `Config Server Settings` - should show the settings used by the client when communicating to the config server.  These have been picked up from the service binding.
* `Config Server Data` - this is the configuration data returned from the Config Servers Git Repo. It will be some of the data from `foo.properties`, `foo-development.properties` and `application.yml` found in the Git repo.
* `Reload` - will cause a reload of the configuration data from the server.

---

### See the [App Configuration](https://steeltoe.io/app-configuration) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
