# Simple - ASP.NET 4.x Sample Application
ASP.NET 4.x sample app illustrating how to use [Spring Cloud Config Server](https://projects.spring.io/spring-cloud/) as a configuration source.

# Pre-requisites

This sample assumes that there is a running Spring Cloud Config Server on your machine. To make this happen

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Visual Stuido 2017
4. Clone the Spring Cloud Config Server repository. (https://github.com/spring-cloud/spring-cloud-config)
5. Go to the config server directory (`spring-cloud-config/spring-cloud-config-server`) and fire it up with `mvn spring-boot:run`
6. This sample will default to looking for its spring cloud config server on localhost, so it should all connect.

The default configuration of the Config Server uses [this github repo]( https://github.com/spring-cloud-samples/config-repo) for its source of configuration data.

# Building & Running on Windows

1. Clone this repo. (i.e. git clone https://github.com/SteeltoeOSS/Samples)
2. Startup Visual Studio
3. Open the src/AspDotNet4/Configuration.sln
4. Select the Simple project in the solution and build it.
5. Select the Simple as the Startup project.
6. Ctl+F5 or F5

# What to expect
After building and running the app, you should see a browser pop up with the home page of the Simple sample app.
Once on the home page, navigate to the `Config Server Data` tab and you'll see the values stored in the github repo used for the Spring Cloud Config Server samples.
If you navigate to the "Config Server Settings" tab you will see the settings used by the Spring Cloud Config server client.

