# Simple - ASP.NET 5 Sample Application
ASP.NET 5 sample app illustrating how to use [Spring Cloud Config Server](http://projects.spring.io/spring-cloud/docs/1.0.3/spring-cloud.html#_spring_cloud_config) as a configuration source.

# Pre-requisites

This sample assumes that there is a running Spring Cloud Config Server on your machine. To make this happen

1. Install Java 8 JDK.
2. Install Maven 3.x.
3. Clone the Spring Cloud Config Server repository. (https://github.com/spring-cloud/spring-cloud-config)
4. Go to the config server directory (`spring-cloud-config/spring-cloud-config-server`) and fire it up with `mvn spring-boot:run`
5. This sample will default to looking for its spring cloud config server on localhost, so it should all connect.

This default configuration of the Config Server uses [this github repo]( https://github.com/spring-cloud-samples/config-repo) for its source of configuration data.

# Building & Running

1. Clone this repo. (i.e. git clone https://github.com/SteelToeOSS/Samples)
2. cd samples/Configuration/src/Simple
3. Install DNX 1.0.0-rc1-final/update1. Install either the coreclr and/or clr runtimes. 
4. Add a DNX runtime to your path. (e.g. dnvm use 1.0.0-rc1-update1 -r clr)
5. dnu restore
6. dnx web

# What to expect
After building and running the app, you should see something like the following:
```
$ cd samples/Configuration/src/Simple
$ dnx web
Hosting environment: Production
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```
Fire up a browser and hit http://localhost:5000.  Once on the home page, navigate to the `Config Server Data` tab and you'll see the values stored in the github repo used for the Spring Cloud Config Server samples.
If you navigate to the "Config Server Settings" tab you will see the settings used by the Spring Cloud Config server client.

