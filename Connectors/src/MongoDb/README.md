# MongoDB Connector Sample App

ASP.NET Core sample app illustrating how to use the [Steeltoe MongoDB Connector](https://docs.steeltoe.io/api/v4/connectors/mongodb.html)
for connecting to a MongoDB database.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   with [Tanzu Cloud Service Broker for Microsoft Azure](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-microsoft-azure/1-13/csb-azure/reference-azure-cosmosdb-mongo.html)
   and [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)
1. Optional: [Tanzu Platform for Kubernetes](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/overview.html) v1.6 or higher
   and [Tanzu CLI](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/install-tanzu-cli.html)

## Running locally

1. Start a MongoDB [docker container](https://github.com/SteeltoeOSS/Samples/blob/4.x/CommonTasks.md)
1. Run the sample
   ```shell
   dotnet run
   ```

Upon startup, the app inserts a couple of objects into the bound MongoDB database. They are displayed on the home page.

## Running on Tanzu Platform for Cloud Foundry

1. Create a MongoDB service instance in an org/space
   ```shell
   cf target -o your-org -s your-space
   cf marketplace
   cf marketplace -e your-offering
   cf create-service csb-azure-mongodb your-plan sampleMongoDbService
   ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs mongodb-connector-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Copy the value of `routes` in the output and open in your browser

## Running on Tanzu Platform for Kubernetes

### Create MongoDB class claim

In order to connect to MongoDB for this sample, you must have a class claim available for the application to bind to.
The commands listed below will create the claim, and the claim will be bound to the application via the definition
in the `workload.yaml` that is included in the `config` folder of this project.

```shell
kubectl config set-context --current --namespace=your-namespace
tanzu service class-claim create sample-mongodb-service --class mongodb-unmanaged
```

If you'd like to learn more about these services, see [claiming services](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/getting-started-claim-services.html)
and [consuming services](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/getting-started-consume-services.html) in the documentation.

### App deployment

To deploy from local source code:
```shell
tanzu app workload apply --local-path . --file ./config/workload.yaml -y
```

Alternatively, from locally built binaries:
```shell
dotnet publish -r linux-x64 --no-self-contained
tanzu app workload apply --local-path ./bin/Release/net8.0/linux-x64/publish --file ./config/workload.yaml -y
```

See the [Tanzu documentation](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/getting-started-deploy-first-app.html) for details.

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
