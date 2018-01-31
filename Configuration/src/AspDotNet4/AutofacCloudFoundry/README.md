# AutofacCloudFoundry - ASP.NET 4.x Sample Application

ASP.NET 4.x sample app built using the Autofac IOC container and illustrating how to use [Config Server for Pivotal Cloud Foundry](http://docs.pivotal.io/spring-cloud-services/config-server/) as a configuration source.

## Pre-requisites

1. Installed Pivotal CloudFoundry with Windows support
1. Installed Spring Cloud Services
1. Visual Studio 2017

## Setup Config Server

You must first create an instance of the Config Server service in a org/space.

1. cf target -o myorg -s development
1. cd src\AspDotNet4\AutofacCloudFoundry
1. cf create-service p-config-server standard myConfigServer -c ./config-server.json
1. Wait for service to be available. (e.g. cf services)

## Publish App & Push

1. Open src\AspDotNet4\Configuration.sln in Visual Studio.
1. Select AutofacCloudFoundry project in Solution Explorer.
1. Right-click and select Publish
1. Publish the App to a folder. (e.g. c:\publish)
1. cd publish_folder (e.g. cd c:\publish)
1. cf push

## What to expect

The cf push will create an app in the space by the name `foo` and will bind the `myConfigServer` service instance to the app. You can hit the app @ `http://foo.x.y.z/`.

The Config Servers Git repository has been set to: `https://github.com/spring-cloud-samples/config-repo`

Use the menus at the top of the app to see various output:

* `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.
* `Config Server Settings` - should show the settings used by the client when communicating to the config server.  These have been picked up from the service binding.
* `Config Server Data` - this is the configuration data returned from the Config Servers Git Repo. It will be some of the data from `foo.properties`, `foo-development.properties` and `application.yml` found in the Git repo.
* `Reload` - will cause a reload of the configuration data from the server.

## Observe Logs

To see the logs as you startup and use the app: `cf logs foo`

You should see something like this during startup:

```text
2016-05-03T12:21:41.30-0600 [STG/0]      OUT Successfully created container
2016-05-03T12:21:41.31-0600 [STG/0]      OUT Downloading app package...
2016-05-03T12:21:44.80-0600 [STG/0]      OUT Downloaded app package (6M)
2016-05-03T12:21:44.80-0600 [STG/0]      OUT Staging...
2016-05-03T12:21:48.46-0600 [STG/0]      OUT Exit status 0
2016-05-03T12:21:48.46-0600 [STG/0]      OUT Staging complete
2016-05-03T12:21:48.46-0600 [STG/0]      OUT Uploading droplet, build artifacts cache...
2016-05-03T12:21:48.46-0600 [STG/0]      OUT Uploading build artifacts cache...
2016-05-03T12:21:48.46-0600 [STG/0]      OUT Uploading droplet...
2016-05-03T12:21:48.55-0600 [STG/0]      OUT Uploaded build artifacts cache (88B)
2016-05-03T12:22:14.02-0600 [STG/0]      OUT Uploaded droplet (5.9M)
2016-05-03T12:22:14.03-0600 [STG/0]      OUT Uploading complete
2016-05-03T12:22:15.59-0600 [CELL/0]     OUT Creating container
2016-05-03T12:22:17.03-0600 [CELL/0]     OUT Successfully created container
2016-05-03T12:22:20.07-0600 [APP/0]      OUT Running ..\tmp\lifecycle\WebAppServer.exe
2016-05-03T12:22:20.12-0600 [APP/0]      OUT PORT == 61990
2016-05-03T12:22:20.12-0600 [APP/0]      OUT 2016-05-03 18:22:20Z|INFO|Port:61990
2016-05-03T12:22:20.12-0600 [APP/0]      OUT 2016-05-03 18:22:20Z|INFO|Webroot:C:\containerizer\F3C11CDF618FAE04DF\user\app
2016-05-03T12:22:20.20-0600 [APP/0]      OUT 2016-05-03 18:22:20Z|INFO|Starting web server instance...
2016-05-03T12:22:20.33-0600 [APP/0]      OUT Server Started.... press CTRL + C to stop

```

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/docs/steeltoe-configuration) for a more in-depth walkthrough of the samples and more detailed information