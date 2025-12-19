# Steeltoe Configuration Providers Sample App

This is an ASP.NET Core application that shows how to use various `IConfiguration` providers that are part of the Steeltoe project.

## General pre-requisites

1. Installed .NET 10 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   with [Spring Cloud Services for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/spring/spring-cloud-services-for-cloud-foundry/3-3/scs-tanzu/index.html)
   and [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)
1. Optional: [Tanzu Platform for Kubernetes](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/overview.html) v1.5 or higher
   with [Application Configuration Service](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/application-configuration-service-for-tanzu/2-4/app-config-service/overview.html)
   and [Tanzu CLI](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/tanzu-application-platform/1-12/tap/install-tanzu-cli.html)

## Running locally

1. Start a Config Server [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Run the sample
   ```shell
   dotnet run
   ```

## Running on Tanzu Platform for Cloud Foundry

### Config Server deployment

Refer to [common tasks](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md#provision-sccs-on-cloud-foundry)
for detailed instructions on creating a Spring Cloud Config Server instance in Cloud Foundry.
This sample expects the config server to be backed by the `spring-cloud-samples` repo.

### App deployment

1. Login to your Cloud Foundry environment and target your org/space
   ```shell
   cf target -o your-org -s your-space
   ```
1. Run the `cf push` command to deploy from source
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net10.0/win-x64/publish
     ```

## Running on Tanzu Platform for Kubernetes

### Config Server deployment

YAML files for creating the needed resources are included with this project, and their usage is specified below,
but you are encouraged to review and/or customize the contents of the files before applying them.

To create configuration objects (ConfigurationSource, ConfigurationSlice, ResourceClaim), run:
```shell
kubectl config set-context --current --namespace=your-namespace
kubectl apply -f ./config/application-configuration-service
```

For complete instructions, follow the [documentation](https://techdocs.broadcom.com/us/en/vmware-tanzu/standalone-components/application-configuration-service-for-tanzu/2-4/app-config-service/overview.html).

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

See the Official [Steeltoe Configuration Documentation](https://docs.steeltoe.io/api/v4/configuration/) for more detailed information.
