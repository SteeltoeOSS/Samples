# ASP.NET Core Data Protection with Redis Keystore Sample App

ASP.NET Core sample app illustrating how to make use of the Steeltoe [DataProtection Key Storage Provider for Redis](https://github.com/SteeltoeOSS/Steeltoe/tree/main/src/Security/src/DataProtection.Redis).
Simplifies using a Redis or Valkey cache on Cloud Foundry for storing encrypted session state.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   and one of the following service brokers:

   - [Redis for Tanzu Application Service](https://techdocs.broadcom.com/us/en/vmware-tanzu/data-solutions/redis-for-tanzu-application-service/3-5/redis-for-tas/index.html)
   - [Tanzu for Valkey on Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/data-solutions/tanzu-for-valkey-on-cloud-foundry/4-0/valkey-on-cf/index.html)
   - [Tanzu Cloud Service Broker for Microsoft Azure](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-microsoft-azure/1-13/csb-azure/reference-azure-redis.html)
   - [Tanzu Cloud Service Broker for AWS](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-aws/1-14/csb-aws/reference-aws-redis.html)

   and [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)

> [!NOTE]
> Versions of ASP.NET Core before v9 require that Lua scripting is activated, which is disabled by default on Cloud Foundry, so check your plan settings.

## Running locally

1. Start a Redis or Valkey [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Run the sample
   ```shell
   dotnet run
   ```

Upon startup, the app displays session information on the home page, something like the following:

| Instance Index | Session ID | Session Value |
|---|---|---|
| N/A | 3f609c90-fd11-19d9-c231-fa988071030f | Example Protected Text - 865765ac-61d2-4ea5-b711-4b6c9123bc6e |

## Running on Tanzu Platform for Cloud Foundry

1. Create a Redis service instance in an org/space
   ```shell
   cf target -o your-org -s your-space
   cf marketplace
   cf marketplace -e your-offering
   ```
   - When using Redis for Tanzu Application Service or Tanzu for Valkey on Cloud Foundry:
     ```shell
     cf create-service p.redis your-plan sampleRedisDataProtectionService
     ```
   - When using Tanzu Cloud Service Broker for Microsoft Azure:
     ```shell
     cf create-service csb-azure-redis your-plan sampleRedisDataProtectionService
     ```
   - When using Tanzu Cloud Service Broker for AWS:
     ```shell
     cf create-service csb-aws-redis your-plan sampleRedisDataProtectionService
     ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs redis-data-protection-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Copy the value of `routes` in the output and open in your browser
1. Scale up to multiple app instances
   ```shell
   cf scale redis-data-protection-sample --instances 2
   ```
1. Wait for the new instance to start up.

Using the same browser session, refresh the page a couple more times.
It may take a few tries to get routed to the second app instance.
When this happens, you should see the `Instance Index` changing, while the `Session ID` and `Session Value` remain the same.

A couple things to note at this point about this app:

* The app is using the Cloud Foundry Redis service to store session data. As a result, the session state is available to all instances of the app.
* The session ID that is in the session cookie and the data that is stored in Redis is encrypted using keys that are now stored in the keyring,
which is also stored in the Cloud Foundry Redis service. So when you scale the app to multiple instances, the same keyring is used by all instances
and therefore the session data can be decrypted by any instance of the application.
* For multiple app instances to share Redis data, ensure they have an identical `name` in the connection string in `appsettings.json`.

---

See the Official [Steeltoe Security Documentation](https://docs.steeltoe.io/api/v4/security/) for more detailed information.
