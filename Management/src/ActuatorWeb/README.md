# ActuatorWeb - a sample web app for Steeltoe Actuators

ASP.NET Core sample app illustrating how to use [Steeltoe Management Endpoints](https://docs.steeltoe.io/api/v3/management/) together with the [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html) on Cloud Foundry and [Spring Boot Admin](https://docs.spring-boot-admin.com/) anywhere else.

This application also illustrates how to have application metrics captured and exported to the [Metrics Registrar](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/metric-registrar-index.html) service so that applications metrics can be viewed in any tool that is able to consume those metrics from the [Cloud Foundry Loggregator](https://github.com/cloudfoundry/loggregator-release).  Several tools exist that can do this, including [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/2.2/app-metrics/GUID-index.html).

> This application is intended to be used with [ActuatorApi](../ActuatorApi/), but can be used on its own if you are looking for a simpler actuator or metrics example.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   * [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
   * [Metrics Registrar](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/metric-registrar-index.html)
   * [App Metrics](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/2.2/app-metrics/GUID-index.html)

## Running locally

This application has a number of moving pieces, some of which can be used in multiple ways. In order to experience all of the functionality in a local environment, you will need to meet additional pre-requisites:

* Installed container orchestrator (such as [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman Desktop](hhttps://podman-desktop.io/))
* Optional: an IDE (such as Visual Studio), for performing the requests defined in [ActuatorWeb.http](./ActuatorWeb.http).

<!-- TODO: add OpenTelemetry content -->

### Spring Boot Admin

Because Steeltoe Management Endpoints are compatible with Spring Boot Actuator, they can also be used with [Spring Boot Admin](https://docs.spring-boot-admin.com/). This application is configured to register itself (as defined in [appsettings.Development.json](./appsettings.Development.json)) with a local server. *For local development purposes only**, use the command below to start a Spring Boot Admin server that will be removed when the container is stopped:

```shell
docker run --rm -it -p 9090:9090 --name steeltoe-SpringBootAdmin steeltoe.azurecr.io/spring-boot-admin
```

\* This image is very basic and support should not be expected. See how the image is built [here](https://github.com/SteeltoeOSS/Dockerfiles/tree/main/spring-boot-admin).

### Zipkin Server

For capturing and reviewing distributed traces, use this command to run a Zipkin server that will be removed when the container is stopped:

```shell
docker run --rm -it -p 9411:9411 --name zipkin openzipkin/zipkin
```

<!-- ### TODO: Grafana Alloy

https://grafana.com/docs/alloy/latest/set-up/install/docker/

https://grafana.com/docs/grafana/latest/datasources/zipkin/

```
docker run --rm -it -p 12345:12345 -p 9090:9090 -v .\config.alloy:/etc/alloy/config.alloy --name grafana-alloy grafana/alloy:latest run --server.http.listen-addr=0.0.0.0:12345 /etc/alloy/config.alloy
```
-->

### Running the application

1. Run the sample, either from your IDE or with this command:

   ```shell
   dotnet run
   ```

 <!-- #### TODO: Orchestrate the complexity with Aspire -->

## Running on Tanzu Platform for Cloud Foundry

1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs actuator-web-management-sample`)
   * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:

     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

1. Copy the value of `routes` in the output and open in your browser

### What to expect

Once the app is up and running, then you can access the management endpoints exposed by Steeltoe using [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html).

Steeltoe exposes Spring Boot Actuator compatible Endpoints which can be accessed via the Tanzu Apps Manager. By using the Apps Manager, you can view the App's Health, Build Information (for example: Git info, etc), as well as view or change the application's logging levels.

Check out the Apps Manager, [Using Spring Boot Actuators](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/using-actuators.html) for more information.

### Install the Metrics Registrar CLI Plugin

1. cf install-plugin -r CF-Community "metric-registrar"

### View Application Metrics in App Metrics

If you wish to collect and view applications metrics in [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/index.html), you must first configure [Metrics Registrar](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html) in the TAS for VMs tile. There is no separate product tile (unlike the metrics forwarder). Once thats complete custom metrics will be collected and automatically exported to the Metrics Forwarder service.  

1. cf target -o myOrg -s development
2. cf register-metrics-endpoint actuator-web-management-sample actuator/metrics
3. cf restart actuator-api-management-sample

To view the metrics you can use the [App Metrics](https://network.pivotal.io/products/apm) tool from Pivotal. Follow the instructions in the [documentation](https://docs.pivotal.io/app-metrics/1-6/index.html) and pay particular attention to the section on viewing [Custom Metrics](https://docs.pivotal.io/app-metrics/1-6/using.html#custom).

---

### See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v3/management/) for more information
