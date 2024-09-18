# ActuatorApi - a sample API app for Steeltoe Actuators

ASP.NET Core sample app illustrating how to use [Steeltoe Management Endpoints](https://docs.steeltoe.io/api/v3/management/) together with the [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html/index.html) for monitoring and managing your applications on Cloud Foundry.  

This application also illustrates how to have application metrics captured and exported to the [Metrics Registrar for PCF](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html) service so that applications metrics can be viewed in any tool that is able to consume those metrics from the [Cloud Foundry Loggregator Firehose](https://docs.pivotal.io/pivotalcf/2-1/loggregator/architecture.html#firehose).  Several tools exist that can do this, including [PCF Metrics](https://docs.pivotal.io/app-metrics/1-6/using.html) from Pivotal.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   (optionally with [Windows support](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/concepts-overview.html))
   with [VMware MySQL for Tanzu Application Service](https://docs.vmware.com/en/VMware-SQL-with-MySQL-for-Tanzu-Application-Service/index.html)
   and [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)

## Running locally

OpenTelemetry something something, Actuators compatible with Spring Boot so use SBA, etc

### Grafana Alloy

https://grafana.com/docs/alloy/latest/set-up/install/docker/

```
docker run --rm -it -p 12345:12345 -v .\config.alloy:/etc/alloy/config.alloy --name grafana-alloy grafana/alloy:latest run --server.http.listen-addr=0.0.0.0:12345 /etc/alloy/config.alloy
```

### MySQL

```
docker run --rm -it -p 3306:3306 --name steeltoe-mysql -e MYSQL_ROOT_PASSWORD=steeltoe -e MYSQL_DATABASE=steeltoe -e MYSQL_USER=steeltoe -e MYSQL_PASSWORD=steeltoe mysql
```

### Spring Boot Admin

Steeltoe [common tasks]()

```
docker run --rm -it -p 9090:9090 --name steeltoe-springbootadmin steeltoe.azurecr.io/spring-boot-admin
```

### Zipkin

## Pre-requisites - CloudFoundry

1. Installed Pivotal Cloud Foundry
2. Installed Apps Manager on Cloud Foundry
3. Installed MySql CloudFoundry service
4. Optionally install [Metrics Registrar for PCF](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html).
5. Optionally install [PCF Metrics](https://network.pivotal.io/products/apm).
6. Install .NET Core SDK

## Install the Metrics Registrar CLI Plugin

1. cf install-plugin -r CF-Community "metric-registrar"

## Running on Tanzu Platform for Cloud Foundry

1. Create a MySQL service instance in an org/space
   ```
   cf target -o your-org -s your-space
   ```
   - When using VMware MySQL for Tanzu Application Service:
     ```
     cf create-service p.mysql db-small sampleMySqlService
     ```
   - When using the Cloud Service Broker for Azure:
     ```
     cf create-service csb-azure-mysql small sampleMySqlService
     ```
   - When using the Cloud Service Broker for GCP:
     ```
     cf create-service csb-google-mysql your-plan sampleMySqlService
     ```
1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs actuator-api-management-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:
     ```
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```
1. Copy the value of `routes` in the output and open in your browser

> Note: The provided manifest will create an app named `actuator-api-management-sample` and attempt to bind to the the app to MySql service `sampleMySqlService`.

## Running Tasks

Depending on the steps taken to push the application to Cloud Foundry, the commands below may require customization (for example, if the application was published before pushing)

Apply Entity Framework database migration scripts as a Cloud Foundry task:

``` shell
cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=MigrateDatabase" 
```

Add forecast data:

``` shell
cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ForecastWeather" 
```

Reset forecast data:

``` shell
cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ResetWeather" 
```

## What to expect - CloudFoundry

Once the app is up and running then you can access the management endpoints exposed by Steeltoe using [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html).

Steeltoe exposes Spring Boot Actuator compatible Endpoints which can be used using the Tanzu Apps Manager. By using the Apps Manager, you can view the App's Health, Build Information (for example: Git info, etc), as well as view or change the application's logging levels.

Check out the Apps Manager, [Using Spring Boot Actuators](https://docs.pivotal.io/pivotalcf/2-1/console/using-actuators.html) for more information.

## View Application Metrics in PCF Metrics

If you wish to collect and view applications metrics in [App Metrics](https://docs.pivotal.io/pcf-metrics/1-4/index.html), you must first configure [Metrics Registrar](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html) in the TAS for VMs tile. There is no separate product tile (unlike the metrics forwarder). Once thats complete custom metrics will be collected and automatically exported to the Metrics Forwarder service.  

1. cf target -o myorg -s development
2. cf register-metrics-endpoint actuator actuator/metrics
3. cf restart actuator

To view the metrics you can use the [App Metrics](https://network.pivotal.io/products/apm) tool from Pivotal. Follow the instructions in the [documentation](https://docs.pivotal.io/app-metrics/1-6/index.html) and pay particular attention to the section on viewing [Custom Metrics](https://docs.pivotal.io/app-metrics/1-6/using.html#custom).

---

### See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v3/management/) for a more in-depth walkthrough of the samples and more detailed information.
