# Redis Connector Sample App

ASP.NET Core sample app illustrating how to use the [Steeltoe Redis Connector](https://docs.steeltoe.io/api/v4/connectors/redis.html)
to connect to a Redis or Valkey server.
This sample uses [StackExchange.Redis](https://www.nuget.org/packages/StackExchange.Redis) and
[Microsoft.Extensions.Caching.Redis](https://www.nuget.org/packages/StackExchange.Redis) to work with the same Redis service.

## General pre-requisites

1. Installed .NET 10 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/elastic-application-runtime/11-0/eart/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/elastic-application-runtime/11-0/eart/toc-tasw-install-index.html))
   and one of the following service brokers:

   - [Tanzu for Valkey on Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-valkey-tanzu-platform/11-0/valkey-tp/index.html)
   - [Tanzu Cloud Service Broker for Microsoft Azure](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/cloud-service-broker-azure/1-15/csb-azure/reference-azure-redis.html)
   - [Tanzu Cloud Service Broker for AWS](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/cloud-service-broker-aws/1-17/csb-aws/reference-aws-redis.html)

   and [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)
1. Optional: [Tanzu Platform for Kubernetes](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/overview.html) v1.5 or higher
   and [Tanzu CLI](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/install-tanzu-cli.html)

> [!NOTE]
> Versions of ASP.NET Core before v9 require that Lua scripting is activated, which is disabled by default on Cloud Foundry, so check your plan settings.

## Running locally

1. Start a Redis or Valkey [docker container](https://github.com/SteeltoeOSS/Samples/blob/4.x/CommonTasks.md)
1. Run the sample
   ```shell
   dotnet run
   ```

Upon startup, the app inserts a couple of key/value pairs into the bound Redis/Valkey cache using both APIs. They are displayed on the home page.

## Running on Tanzu Platform for Cloud Foundry

1. Create a Redis service instance in an org/space
   ```shell
   cf target -o your-org -s your-space
   cf marketplace
   cf marketplace -e your-offering
   ```
   - When using Tanzu for Valkey on Cloud Foundry:
     ```shell
     cf create-service p.redis your-plan sampleRedisService --wait
     ```
   - When using Tanzu Cloud Service Broker for Microsoft Azure:
     ```shell
     cf create-service csb-azure-redis your-plan sampleRedisService --wait
     ```
   - When using Tanzu Cloud Service Broker for AWS:
     ```shell
     cf create-service csb-aws-redis your-plan sampleRedisService --wait
     ```
1. Deploy the app

   - **From Source:**

     ```shell
     cf push
     ```

   - **From Binaries** (required if deploying to Windows):

     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net10.0/win-x64/publish
     ```

   For either deployment option, monitor startup logs with: `cf logs redis-connector-sample`.
1. Copy the value of `routes` in the output and open in your browser

## Running on Tanzu Platform for Kubernetes

### Create Redis class claim

In order to connect to Redis for this sample, you must have a class claim available for the application to bind to.
The commands listed below will create the claim, and the claim will be bound to the application via the definition
in the `workload.yaml` that is included in the `config` folder of this project.

```shell
kubectl config set-context --current --namespace=your-namespace
tanzu service class-claim create example-redis-service --class redis-unmanaged
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
tanzu app workload apply --local-path ./bin/Release/net10.0/linux-x64/publish --file ./config/workload.yaml -y
```

See the [Tanzu documentation](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/getting-started-deploy-first-app.html) for details.

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
