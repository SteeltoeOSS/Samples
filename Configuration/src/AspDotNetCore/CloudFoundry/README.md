# CloudFoundry - ASP.NET Core Sample Application

ASP.NET Core sample app illustrating how to use the Steeltoe [CloudFoundry](https://github.com/SteeltoeOSS/Configuration/tree/master/src/Steeltoe.Extensions.Configuration.CloudFoundry) configuration provider to parse the VCAP_* environment variables and add them as a configuration source.

## Pre-requisites

1. Installed Pivotal CloudFoundry
1. .NET Core SDK 2.1.300

## Publish App & Push

1. cf target -o myorg -s development
1. cd src/AspDotNetCore/CloudFoundry
1. dotnet restore --configfile nuget.config
1. Publish app to a directory selecting the framework and runtime you want to run on. (e.g. `dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64`)
1. Push the app using the appropriate manifest. (e.g. `cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish` or `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish`)

## What to expect

The cf push will create an app in the space by the name `cloud`. You can hit the app @ `http://cloud.x.y.z/`.

Use the menus at the top of the app to see various output:

* `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.

## Observe Logs

To see the logs as you startup and use the app: `cf logs cloud`

On a Linux cell, you should see something like this during startup:

```text
   2017-08-15T09:59:12.39-0600 [CELL/0] OUT Creating container
   2017-08-15T09:59:12.85-0600 [CELL/0] OUT Successfully created container
   2017-08-15T09:59:13.73-0600 [STG/0] OUT Successfully destroyed container
   2017-08-15T09:59:15.58-0600 [CELL/0] OUT Starting health monitoring of container
   2017-08-15T09:59:17.08-0600 [APP/PROC/WEB/0] OUT Content root path: /home/vcap/app
   2017-08-15T09:59:17.08-0600 [APP/PROC/WEB/0] OUT Hosting environment: Development
   2017-08-15T09:59:17.08-0600 [APP/PROC/WEB/0] OUT Application started. Press Ctrl+C to shut down.
   2017-08-15T09:59:17.08-0600 [APP/PROC/WEB/0] OUT Now listening on: http://0.0.0.0:8080
   2017-08-15T09:59:17.69-0600 [CELL/0] OUT Container became healthy

```

---

### See the Official [Steeltoe Configuration Documentation](https://steeltoe.io/docs/steeltoe-configuration) for a more in-depth walkthrough of the samples and more detailed information