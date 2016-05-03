# SimpleCloudFoundry - ASP.NET 5 Sample Application 
ASP.NET 5 sample app illustrating how to use [Config Server for Pivotal Cloud Foundry](http://docs.pivotal.io/spring-cloud-services/config-server/) as a configuration source.

# Pre-requisites
1. Installed Pivotal CloudFoundry 1.7
2. Installed Spring Cloud Services 1.0.8

# Configuring Config Server & Pushing App
You must first create an instance of the Config Server service in a org/space.
1. cf target -o myorg -s development
2. cd src/SimpleCloudFoundry
3. cf create-service p-config-server standard myConfigServer -c ./config-server.json
4. cf push

# What to expect
The cf push will create an app in the space by the name `foo` and will bind the `myConfigServer` service instance to the app. You can hit the app @ `http://foo.x.y.z/`.

The Config Servers Git repository has been set to: `https://github.com/spring-cloud-samples/config-repo`

Use the menus at the top of the app to see various output:

* `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.
* `Config Server Settings` - should show the settings used by the client when communicating to the config server.  These have been picked up from the service binding.
* `Config Server Data` - this is the configuration data returned from the Config Servers Git Repo. It will be some of the data from `foo.properties`, `foo-development.properties` and `application.yml` found in the Git repo.
* `Reload` - will cause a reload of the configuration data from the server.

# Observe Logs
To see the logs as you startup and use the app: `cf logs foo`

You should see something like this during startup:
```
2016-05-03T10:26:59.86-0600 [CELL/0]     OUT Creating container
2016-05-03T10:27:00.25-0600 [CELL/0]     OUT Successfully created container
2016-05-03T10:27:09.06-0600 [CELL/0]     OUT Starting health monitoring of container
2016-05-03T10:27:09.14-0600 [APP/0]      OUT Adding /app/.dnx/runtimes/dnx-coreclr-linux-x64.1.0.0-rc1-final/bin to process PATH
2016-05-03T10:27:17.73-0600 [APP/0]      OUT info: SteelToe.Extensions.Configuration.ConfigServer.ConfigServerConfigurationProvider[0]
2016-05-03T10:27:17.73-0600 [APP/0]      OUT       Fetching config from server at: https://config-de211817-2e99-4c57-89e8-31fa7ca6a276.apps.testcloud.com
2016-05-03T10:27:20.21-0600 [APP/0]      OUT info: SteelToe.Extensions.Configuration.ConfigServer.ConfigServerConfigurationProvider[0]
2016-05-03T10:27:20.21-0600 [APP/0]      OUT       Located environment: foo, development, master, 
2016-05-03T10:27:20.59-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-03T10:27:20.59-0600 [APP/0]      OUT       Hosting starting
2016-05-03T10:27:20.59-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[4]
2016-05-03T10:27:20.59-0600 [APP/0]      OUT       Hosting starting
2016-05-03T10:27:20.66-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-03T10:27:20.66-0600 [APP/0]      OUT       Hosting started
2016-05-03T10:27:20.66-0600 [APP/0]      OUT verb: Microsoft.AspNet.Hosting.Internal.HostingEngine[5]
2016-05-03T10:27:20.66-0600 [APP/0]      OUT       Hosting started
2016-05-03T10:27:20.66-0600 [APP/0]      OUT Hosting environment: development
2016-05-03T10:27:20.66-0600 [APP/0]      OUT Now listening on: http://0.0.0.0:8080
2016-05-03T10:27:20.66-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-05-03T10:27:21.13-0600 [CELL/0]     OUT Container became healthy

```

