# Steeltoe Configuration Providers Sample App

This is an ASP.NET Core application that shows how to use various `IConfiguration` providers that are part of the Steeltoe project.


## General Pre-Requisites

In order to run this sample locally, you will need a .NET SDK capable of building a target framework of .NET 8.0 or higher.
In order to add Spring Cloud Config Server, we recommend using a docker container.


## Running Locally

Pre-requisites:

1. Installed .NET SDK that supports .NET 8.0

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Config-Server) for instructions on starting a Spring Cloud Config Server.

## Running on Cloud Foundry or Tanzu Application Service (TAS)

Pre-requisites:

1. Access to a Cloud Foundry based environment (such as TAS)
1. Installed Spring Cloud Services
1. Installed [cf cli](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)

### Config Service Deployment on TAS

Refer to [common tasks](/CommonTasks.md#Spring-Cloud-Config-Server#provision-sccs-on-cloud-foundry) for detailed instructions on creating a Spring Cloud Config Server instance in Cloud Foundry. This sample expects the config server to be backed by the `spring-cloud-samples` repo.

### Deploy to Linux

The [dotnet_core_buildpack](https://github.com/cloudfoundry/dotnet-core-buildpack) can build this application from source, so the process to deploy should be straightforward to follow from your preferred shell:

1. Login to your Cloud Foundry environment and target your org/space:
   - `cf target -o your-org -s your-space`

1. Move to the directory containing this sample:
   - `cd Samples/Configuration/src/Steeltoe.Samples.Configuration`

1. Push the app using the appropriate manifest:
   - `cf push -f manifest.yml`

### Deploy to Windows

Additional prerequisites for this option:


1. [Cloud Foundry support for Windows](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/5.0/tas-for-vms/windows-index.html)
1. Installed .NET SDK that supports .NET 8.0

> [!NOTE]
> Because there is no buildpack for Cloud Foundry that can build .NET applications from source for deployment to Windows, the sample must be precompiled before deployment.


1. Login to your Cloud Foundry environment and target your org/space:
   - `cf target -o your-org -s your-space`

1. Move to the directory containing this sample:
   - `cd Samples/Configuration/src/Steeltoe.Samples.Configuration`
1. Run `dotnet publish -r win-x64 --self-contained`
1. Push the binary using the Windows manifest:
   - `cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish/`

## Running on Tanzu Application Platform (TAP)

Pre-requisites:

1. Kubernetes with [Tanzu Application Platform](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/index.html) installed
1. [Application Configuration Service](https://docs.vmware.com/en/Application-Configuration-Service-for-VMware-Tanzu/index.html) installed

<!-- TODO: confirm if the standard https://tanzu.academy/guides/developer-sandbox will work for this -->

### Config Service Deployment on TAP

YAML files for creating the needed resources are included with this project, and their usage is specified below, but you are encouraged to review and/or customize the contents of the files before applying them.

1. Create Configuration objects (ConfigurationSource, ConfigurationSlice, ResourceClaim)
   - For complete instructions, follow the [documentation](https://docs.vmware.com/en/Application-Configuration-Service-for-VMware-Tanzu/index.html)
   - For a simplified experience, use the YAML included with this project: `kubectl apply -f ./config/application-configuration-service`
   - Note that the binding of the resource claim to the workload is activated by the serviceClaims section of the [workload.yaml](./config/workload.yaml)

<!-- ### TODO Deploy to TAP from github-->

### Publish locally and/or deploy to TAP

1. Move to the directory containing this sample:
   - `cd Samples/Configuration/src/Steeltoe.Samples.Configuration`
1. Publish app to a local directory, specifying the runtime:
   - `dotnet publish -r linux-x64 --no-self-contained`
1. Push the app to TAP:
   - `tanzu app workload apply --local-path ./bin/Debug/net8.0/linux-x64/publish --file ./config/workload.yaml -y`

   - See the Tanzu [documentation](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/1.8/tap/getting-started-deploy-first-app.html) for details.

### See the Official [Steeltoe Configuration Documentation](https://docs.steeltoe.io/api/v3/configuration/) for a more in-depth walkthrough of the samples and more detailed information.