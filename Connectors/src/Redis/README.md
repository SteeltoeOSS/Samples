# Redis Connector Sample App

ASP.NET Core sample app illustrating how to use the [Steeltoe Redis Connector](https://docs.steeltoe.io/api/v4/connectors/redis.html)
to connect to a Redis or Valkey server.
This sample uses both [Microsoft RedisCache](https://learn.microsoft.com/dotnet/api/microsoft.extensions.caching.redis.rediscache)
and [StackExchange `ConnectionMultiplexer`](https://github.com/StackExchange/StackExchange.Redis) to work with the same Redis service.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   and one of the following service brokers:

   - [Redis for Tanzu Application Service](https://techdocs.broadcom.com/us/en/vmware-tanzu/data-solutions/redis-for-tanzu-application-service/3-5/redis-for-tas/index.html)
   - [Tanzu for Valkey on Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/data-solutions/tanzu-for-valkey-on-cloud-foundry/4-0/valkey-on-cf/index.html)
   - [Tanzu Cloud Service Broker for Microsoft Azure](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-microsoft-azure/1-12/csb-azure/reference-azure-redis.html)
   - [Tanzu Cloud Service Broker for GCP](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-gcp/1-5/csb-gcp/reference-gcp-redis.html)

   and [Cloud Foundry CLI](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/6-0/tpcf/cf-cli-index.html)
1. Optional: [Tanzu Platform for Kubernetes](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/overview.html) v1.5 or higher
   and [Kubernetes](https://kubernetes.io/docs/tasks/tools/)

> [!NOTE]
> Versions of ASP.NET before v9 require that Lua scripting is activated, which is disabled by default on Cloud Foundry, so check your plan settings.

## Running locally

1. Start a Redis or Valkey [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Run the sample
   ```
   dotnet run
   ```

Upon startup, the app inserts a couple of key/value pairs into the bound Redis/Valkey cache using both APIs. They are displayed on the home page.

## Running on Tanzu Platform for Cloud Foundry

1. Create a Redis service instance in an org/space
   ```
   cf target -o your-org -s your-space
   ```
   - When using Redis for Tanzu Application Service or Tanzu for Valkey on Cloud Foundry:
     ```
     cf create-service p.redis on-demand-cache sampleRedisService
     ```
     or:
     ```
     cf create-service p-redis shared-vm sampleRedisService
     ```
   - When using Tanzu Cloud Service Broker for Microsoft Azure:
     ```
     cf create-service csb-azure-redis your-plan sampleRedisService
     ```
   - When using Tanzu Cloud Service Broker for GCP:
     ```
     cf create-service csb-google-redis your-plan sampleRedisService
     ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs redis-connector-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Copy the value of `routes` in the output and open in your browser

## Running on Tanzu Platform for Kubernetes

### Create Redis class claim

In order to connect to Redis for this sample, you must have a class claim available for the application to bind to.
The commands listed below will create the claim, and the claim will be bound to the application via the definition
in the `workload.yaml` that is included in the `config` folder of this project.

```
kubectl config set-context --current --namespace=your-namespace
tanzu service class-claim create my-postgresql-service --class postgresql-unmanaged
```

If you'd like to learn more about these services, see [claiming services](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.5/tap/getting-started-claim-services.html)
and [consuming services](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.5/tap/getting-started-consume-services.html) in the documentation.

### App deployment

To deploy from local source code:
```
tanzu app workload apply --local-path . --file ./config/workload.yaml -y
```

Alternatively, from locally built binaries:
```
dotnet publish -r linux-x64 --no-self-contained
tanzu app workload apply --local-path ./bin/Release/net8.0/linux-x64/publish --file ./config/workload.yaml -y
```

See the [Tanzu documentation](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/application-configuration-service-for-tanzu/2-4/app-config-service/gettingstarted-index.html) for details.

---

See the Official [Steeltoe Connectors Documentation](https://docs.steeltoe.io/api/v4/connectors/) for more detailed information.
