# SimpleCloudFoundry - ASP.NET Core Sample Application

ASP.NET Core sample app illustrating how to use [Config Server for Pivotal Cloud Foundry](http://docs.pivotal.io/spring-cloud-services/config-server/) as a configuration source.

## Pre-requisites

1. Installed Pivotal CloudFoundry 1.10+
1. Installed Spring Cloud Services 1.1+
1. .Net Core SDK 2.1.300

## Setup Config Server

You must first create an instance of the Config Server service in a org/space.

1. cf target -o myorg -s development
1. cd src/AspDotNetCore/SimpleCloudFoundry
1. cf create-service p-config-server standard myConfigServer -c ./config-server.json
1. Wait for service to become ready (i.e. cf services )

## Publish App & Push

1. cf target -o myorg -s development
1. cd src/AspDotNetCore/SimpleCloudFoundry
1. dotnet restore --configfile nuget.config
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`)

> Note: If you are using self-signed certificates it is possible that you might run into SSL certificate validation issues when pushing this app. The simplest way to fix this:

1. Disable certificate validation for the Spring Cloud Config Client.  You can do this by editing `appsettings.json` and add `spring:cloud:config:validateCertificates=false`.

## What to expect

The cf push will create an app in the space by the name `foo` and will bind the `myConfigServer` service instance to the app. You can hit the app @ `http://foo.x.y.z/`.

The Config Servers Git repository has been set to: `https://github.com/spring-cloud-samples/config-repo`

Use the menus at the top of the app to see various output:

* `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.
* `Config Server Settings` - should show the settings used by the client when communicating to the config server.  These have been picked up from the service binding.
* `Config Server Data` - this is the configuration data returned from the Config Servers Git repository. It will be some of the data from `foo.properties`, `foo-development.properties` and `application.yml` found in the Git repository.
* `Reload` - will cause a reload of the configuration data from the server.

## Observe Logs

To see the logs as you startup and use the app: `cf logs foo`

On a Linux cell, you should see something like this during startup:

```text
2016-06-01T09:14:14.38-0600 [CELL/0]     OUT Creating container
2016-06-01T09:14:15.93-0600 [CELL/0]     OUT Successfully created container
2016-06-01T09:14:17.14-0600 [CELL/0]     OUT Starting health monitoring of container
2016-06-01T09:14:18.01-0600 [APP/0]      OUT info: Steeltoe.Extensions.Configuration.ConfigServer.ConfigServerConfigurationProvider[0]
2016-06-01T09:14:18.01-0600 [APP/0]      OUT       Fetching config from server at: https://config-92e894b5-17e2-4b94-941e-a544c6488de7.apps.testcloud.com
2016-06-01T09:14:19.59-0600 [APP/0]      OUT info: Steeltoe.Extensions.Configuration.ConfigServer.ConfigServerConfigurationProvider[0]
2016-06-01T09:14:19.59-0600 [APP/0]      OUT       Located environment: foo, development, master,
2016-06-01T09:14:19.59-0600 [APP/0]      OUT info: Steeltoe.Extensions.Configuration.ConfigServer.ConfigServerConfigurationProvider[0]
2016-06-01T09:14:19.59-0600 [APP/0]      OUT       Fetching config from server at: https://config-92e894b5-17e2-4b94-941e-a544c6488de7.apps.testcloud.com
2016-06-01T09:14:20.46-0600 [APP/0]      OUT info: Steeltoe.Extensions.Configuration.ConfigServer.ConfigServerConfigurationProvider[0]
2016-06-01T09:14:20.46-0600 [APP/0]      OUT       Located environment: foo, development, master,
2016-06-01T09:14:20.93-0600 [APP/0]      OUT dbug: Microsoft.AspNetCore.Hosting.Internal.WebHost[3]
2016-06-01T09:14:20.93-0600 [APP/0]      OUT       Hosting starting
2016-06-01T09:14:21.04-0600 [APP/0]      OUT dbug: Microsoft.AspNetCore.Hosting.Internal.WebHost[4]
2016-06-01T09:14:21.04-0600 [APP/0]      OUT       Hosting started
2016-06-01T09:14:21.04-0600 [APP/0]      OUT Hosting environment: development
2016-06-01T09:14:21.04-0600 [APP/0]      OUT Content root path: /home/vcap/app
2016-06-01T09:14:21.04-0600 [APP/0]      OUT Now listening on: http://*:8080
2016-06-01T09:14:21.04-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-06-01T09:14:21.41-0600 [CELL/0]     OUT Container became healthy

```

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/docs/steeltoe-configuration) for a more in-depth walkthrough of the samples and more detailed information