# Cloud Foundry - ASP.NET Core Sample Application

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Samples/SteeltoeOSS.Samples%20%5BConfiguration_CloudFoundry%5D?branchName=2.x)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=15&branchName=master)

This ASP.NET Core sample app illustrates how to use the [Steeltoe Cloud Foundry configuration provider](https://steeltoe.io/app-configuration/get-started/cloudfoundry) to parse the VCAP_* environment variables and add them as a configuration source.

## Pre-requisites

1. Installed Pivotal CloudFoundry
1. .NET Core SDK

## Publish App & Push

1. `cf target -o myorg -s development`
1. `cd src/CloudFoundry`
1. `dotnet restore`
1. Publish app to a directory selecting the framework and runtime you want to run on:
   - `dotnet publish -f netcoreapp3.1 -r linux-x64`
1. Push the app using the appropriate manifest:
   - `cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish`
   - `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish`

## What to expect

The `cf push` will create an app in the space by the name `cloud`. You can hit the app @ `https://cloud.x.y.z/`.

Use the menus at the top of the app to see various output:

- `CloudFoundry Settings` - should show `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.

---

### See the [App Configuration](https://steeltoe.io/app-configuration) area of the Steeltoe site for a more in-depth walkthrough of the samples and more detailed information
