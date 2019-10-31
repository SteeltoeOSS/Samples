# Simple - ASP.NET MVC Sample Application

ASP.NET MVC sample illustrating how to use [Spring Cloud Config Server](https://projects.spring.io/spring-cloud/) as a configuration source.

## Pre-requisites

A local config server is required for this sample. Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Config-Server) for detailed instructions on setting one up. This sample expects the config server to be backed by the `spring-cloud-samples` repo.

## Building & Running on Windows

This is a typical ASP.NET MVC project that runs on the 4.x line of .NET Framework.

1. Use Visual Studio to open the solution file `Configuration/src/AspDotNet4/Configuration.sln`
1. Select the Simple project in the solution and build it.
1. Right click on the `Simple` project.
1. Select `Debug -> Start New Instance`

## What to expect

After starting the app, you should see a browser pop up with the home page of the Simple sample app.
Once on the home page, navigate to the `Config Server Data` tab and you'll see the values stored in the github repository used for the Spring Cloud Config Server samples.
If you navigate to the "Config Server Settings" tab you will see the settings used by the Spring Cloud Config server client.

---

### See the [App Configuration](https://steeltoe.io/app-configuration) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
