# Steeltoe Configuration Providers Sample App

This is an ASP.NET Core application that shows how to use various `IConfiguration` providers that are part of the Steeltoe project.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   (optionally with [Windows support](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/concepts-overview.html))
   with [Spring Cloud Services for VMware Tanzu](https://docs.vmware.com/en/Spring-Cloud-Services-for-VMware-Tanzu/index.html)
   and [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
1. Optional: [VMware Tanzu Platform for Kubernetes](https://docs.vmware.com/en/VMware-Tanzu-Platform/services/create-manage-apps-tanzu-platform-k8s/overview.html) v1.5 or higher
   with [Application Configuration Service](https://docs.vmware.com/en/Application-Configuration-Service-for-VMware-Tanzu/index.html)
   and [Kubernetes](https://kubernetes.io/docs/tasks/tools/)

## Running locally

1. Start a Config Server [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)
1. Run the sample
   ```
   dotnet run
   ```

## Running on Tanzu Platform for Cloud Foundry

### Config Server deployment

Refer to [common tasks](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md#provision-sccs-on-cloud-foundry)
for detailed instructions on creating a Spring Cloud Config Server instance in Cloud Foundry.
This sample expects the config server to be backed by the `spring-cloud-samples` repo.

### App deployment

1. Login to your Cloud Foundry environment and target your org/space
   ```
   cf target -o your-org -s your-space
   ```
1. Run the `cf push` command to deploy from source
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

## Running on Tanzu Platform for Kubernetes

### Config Server deployment

YAML files for creating the needed resources are included with this project, and their usage is specified below,
but you are encouraged to review and/or customize the contents of the files before applying them.

To create configuration objects (ConfigurationSource, ConfigurationSlice, ResourceClaim), run:
```
kubectl config set-context --current --namespace=your-namespace
kubectl apply -f ./config/application-configuration-service
```

For complete instructions, follow the [documentation](https://docs.vmware.com/en/Application-Configuration-Service-for-VMware-Tanzu/index.html).

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

See the [Tanzu documentation](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.8/tap/getting-started-deploy-first-app.html) for details.

---

See the Official [Steeltoe Configuration Documentation](https://docs.steeltoe.io/api/v3/configuration/) for a more in-depth walkthrough of the samples and more detailed information.
