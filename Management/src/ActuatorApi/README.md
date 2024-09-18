# ActuatorApi - a sample API app for Steeltoe Actuators

ASP.NET Core sample app illustrating how to use [Steeltoe Management Endpoints](https://docs.steeltoe.io/api/v3/management/) together with the [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html) on Cloud Foundry and [Spring Boot Admin](https://docs.spring-boot-admin.com/) anywhere else.

This application also illustrates how to have application metrics captured and exported to the [Metrics Registrar](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/metric-registrar-index.html) service so that applications metrics can be viewed in any tool that is able to consume those metrics from the [Cloud Foundry Loggregator](https://github.com/cloudfoundry/loggregator-release).  Several tools exist that can do this, including [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/2.2/app-metrics/GUID-index.html).

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [VMware Tanzu Platform for Cloud Foundry](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/index.html)
   * [Cloud Foundry CLI](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
   * [VMware MySQL for Tanzu Application Service](https://docs.vmware.com/en/VMware-SQL-with-MySQL-for-Tanzu-Application-Service/index.html) or [VMware Tanzu Cloud Service Broker](https://docs.vmware.com/en/Cloud-Service-Broker-for-VMware-Tanzu/index.html)
   * [Metrics Registrar](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/metric-registrar-index.html)
   * [App Metrics](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/2.2/app-metrics/GUID-index.html)

## Running locally

This application has a number of moving pieces, some of which can be used in multiple ways. In order to experience all of the functionality in a local environment, you will need to meet additional pre-requisites:

* Installed container orchestrator (such as [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman Desktop](hhttps://podman-desktop.io/))
* Optional: an IDE (such as Visual Studio), for performing the requests defined in [ActuatorApi.http](./ActuatorApi.http).

<!-- TODO: add OpenTelemetry content -->

### MySQL

This application depends on a MySQL database. The command below will start a server that is configured with credentials defined in [appsettings.Development.json](./appsettings.Development.json) and set to be removed when the container is stopped:

```shell
docker run --rm -it -p 3306:3306 --name steeltoe-mysql -e MYSQL_ROOT_PASSWORD=steeltoe -e MYSQL_DATABASE=steeltoe -e MYSQL_USER=steeltoe -e MYSQL_PASSWORD=steeltoe mysql
```

Be sure to update the connection string if using another MySQL configuration.

### Administrative Tasks (initialize the database)

In order to demonstrate [Steeltoe Management Tasks](https://docs.steeltoe.io/api/v3/management/tasks.html), the database schema and its contents are managed as administrative tasks. 

Apply Entity Framework database schema migration scripts:

```shell
dotnet run --runtask=MigrateDatabase
```

[Add forecast data](./AdminTasks/ForecastTask.cs) (if not provided, `fromDate` will be set to the current date and `` will be 7):

```shell
dotnet run --runtask=ForecastWeather --fromDate=10-18-2024 --days=30
```

[Reset forecast data](./AdminTasks/ResetTask.cs):

```shell
dotnet run --runtask=ResetWeather
```

### Spring Boot Admin

Because Steeltoe Management Endpoints are compatible with Spring Boot Actuator, they can also be used with [Spring Boot Admin](https://docs.spring-boot-admin.com/). This application is configured to register itself (as defined in [appsettings.Development.json](./appsettings.Development.json)) with a local server. *For local development purposes only**, use the command below to start a Spring Boot Admin server that will be removed when the container is stopped:

```script
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
docker run --rm -it -p 12345:12345 -v .\config.alloy:/etc/alloy/config.alloy --name grafana-alloy grafana/alloy:latest run --server.http.listen-addr=0.0.0.0:12345 /etc/alloy/config.alloy
``` -->

### Running the application

1. Run the sample, either from your IDE or with this command:

   ```shell
   dotnet run
   ```

 <!-- #### TODO: Orchestrate the complexity with Aspire -->

## Running on Tanzu Platform for Cloud Foundry

1. Create a MySQL service instance in an org/space

   ```shell
   cf target -o your-org -s your-space
   ```

   * When using VMware MySQL for Tanzu Application Service:

     ```shell
     cf create-service p.mysql db-small sampleMySqlService
     ```

   * When using the Cloud Service Broker for Azure:

     ```shell
     cf create-service csb-azure-mysql small sampleMySqlService
     ```

   * When using the Cloud Service Broker for GCP:

     ```shell
     cf create-service csb-google-mysql your-plan sampleMySqlService
     ```

1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs actuator-api-management-sample`)
   * When deploying to Windows, binaries must be built locally before push. Use the following commands instead:

     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

1. Copy the value of `routes` in the output and open in your browser

> Note: The provided manifest will create an app named `actuator-api-management-sample` and attempt to bind to the the app to MySql service `sampleMySqlService`.

### Running Tasks

Depending on the steps taken to push the application to Cloud Foundry, the commands below may require customization (for example, if the application was published before pushing, the path in the command should be shortened to `./Steeltoe.Samples.ActuatorApi`)

Apply Entity Framework database migration scripts as a Cloud Foundry task:

```shell
cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=MigrateDatabase" 
```

Add forecast data:

```shell
cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ForecastWeather" 
```

Reset forecast data:

```shell
cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ResetWeather" 
```

### What to expect

Once the app is up and running, then you can access the management endpoints exposed by Steeltoe using [Apps Manager](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/manage-apps.html).

Steeltoe exposes Spring Boot Actuator compatible Endpoints which can be accessed via the Tanzu Apps Manager. By using the Apps Manager, you can view the App's Health, Build Information (for example: Git info, etc), as well as view or change the application's logging levels.

Check out the Apps Manager, [Using Spring Boot Actuators](https://docs.vmware.com/en/VMware-Tanzu-Application-Service/6.0/tas-for-vms/using-actuators.html) for more information.

### Install the Metrics Registrar CLI Plugin

1. cf install-plugin -r CF-Community "metric-registrar"

### View Application Metrics in App Metrics

If you wish to collect and view applications metrics in [App Metrics for VMware Tanzu](https://docs.vmware.com/en/App-Metrics-for-VMware-Tanzu/index.html), you must first configure [Metrics Registrar](https://docs.pivotal.io/platform/application-service/2-9/metric-registrar/index.html) in the TAS for VMs tile. There is no separate product tile (unlike the metrics forwarder). Once thats complete custom metrics will be collected and automatically exported to the Metrics Forwarder service.  

1. cf target -o myOrg -s development
2. cf register-metrics-endpoint actuator-api-management-sample actuator/metrics
3. cf restart actuator-api-management-sample

To view the metrics you can use the [App Metrics](https://network.pivotal.io/products/apm) tool from Pivotal. Follow the instructions in the [documentation](https://docs.pivotal.io/app-metrics/1-6/index.html) and pay particular attention to the section on viewing [Custom Metrics](https://docs.pivotal.io/app-metrics/1-6/using.html#custom).

---

### See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v3/management/) for more information.
