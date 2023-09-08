# MongoDB Connector Sample App

ASP.NET Core sample app illustrating how to use [Steeltoe MongoDB Connector](https://docs.steeltoe.io/api/v3/connectors/mongodb.html) for connecting to a MongoDB service on CloudFoundry.

## General prerequisites

1. Installed .NET Core SDK

## Running locally

1. Started MongoDB [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)

## Running on CloudFoundry

Pre-requisites:

1. Installed CloudFoundry (optionally with Windows support)
1. Installed [VMware Tanzu Cloud Service Broker](https://docs.vmware.com/en/Cloud-Service-Broker-for-VMware-Tanzu/index.html)

### Create MongoDB Service Instance on CloudFoundry

You must first create an instance of the MongoDB service in an org/space.

1. `cf target -o your-org -s your-space`
1. `cf create-service csb-azure-mongodb small myMongoDbService`

### Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
1. `cd samples/Connectors/src/MongoDb`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       - `dotnet restore --configfile nuget.config`
       - `dotnet publish -r win-x64 --self-contained`
     - Push the app using the appropriate manifest:
       - `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       - `cf push -f manifest.yml`

> Note: The provided manifest will create an app named `mongodb-connector` and attempt to bind the app to MongoDB service `myMongoDbService`.

### What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs mysql-connector`

On a Windows cell, you should see something like this during startup:

```text
2016-07-01T07:27:49.73-0600 [CELL/0]     OUT Creating container
2016-07-01T07:27:51.11-0600 [CELL/0]     OUT Successfully created container
2016-07-01T07:27:54.49-0600 [APP/0]      OUT Running .\MongoDb
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Hosting environment: development
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Content root path: C:\containerizer\3737940917E4D13A25\user\app
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Now listening on: http://*:57540
2016-07-01T07:27:57.73-0600 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
```

This sample will be available at <http://mongodb-connector.[your-cf-apps-domain]/>.

Upon startup, the app inserts a couple of objects into the bound MongoDB database. They are displayed on the home page.

## Running on Tanzu Application Platform (TAP)

Pre-requisites:

1. Kubernetes with [Tanzu Application Platform v1.6 or higher](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/index.html) installed

### Create MongoDB Class Claim on TAP

In order to connect to MongoDB on TAP for this sample, you must have a class claim available for the application to bind to. The commands listed below will create the claim, and the claim will be bound to the application via the definition in the workload.yaml that is included in the `config` folder of this project. 

1. `kubectl config set-context --current --namespace=your-namespace`
1. `tanzu service class-claim create my-mongodb-service --class mongodb-unmanaged`

If you'd like to learn more about these services, see [claiming services](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.5/tap/getting-started-claim-services.html) and [consuming services](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.5/tap/getting-started-consume-services.html) in the TAP documentation.

### Publish App & Push to TAP

1. `kubectl config set-context --current --namespace=your-namespace`
1. `cd samples/Connectors/src/MongoDb`
1. Push the app
   - From local source code:
     - Push the app using the appropriate workload.yaml:
       - `tanzu app workload apply --local-path . --source-image your-registry-reference --file ./config/workload.yaml -y`
   - Alternatively, from locally built binaries:
     - Publish app to a local directory, specifying the runtime:
       - `dotnet restore --configfile nuget.config`
       - `dotnet publish -r linux-x64 --no-self-contained`
     - Push the app using the appropriate workload.yaml:
       - `tanzu app workload apply --local-path ./bin/Debug/net6.0/linux-x64/publish --source-image your-registry-reference --file ./config/workload.yaml -y`

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
