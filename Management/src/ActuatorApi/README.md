# Steeltoe Management Sample - Actuators, Administrative Tasks, Metrics and Tracing

ActuatorWeb and ActuatorApi form an ASP.NET Core-powered sample application that demonstrates how to use several Steeltoe libraries on their own and with additional tools.

In order to avoid duplicating a significant amount of content, the [ActuatorWeb Readme](../ActuatorWeb/README.md) contains the shared information and this document only holds content unique to ActuatorApi.

## General pre-requisites

1. Installed .NET 8 SDK
1. Optional: [Tanzu Platform for Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/concepts-overview.html)
   (optionally with [Windows support](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform/tanzu-platform-for-cloud-foundry/10-0/tpcf/toc-tasw-install-index.html))
   and one of the following service brokers:

   - [Tanzu for MySQL on Cloud Foundry](https://techdocs.broadcom.com/us/en/vmware-tanzu/data-solutions/tanzu-for-mysql-on-cloud-foundry/10-0/mysql-for-tpcf/use.html)
   - [Tanzu Cloud Service Broker for GCP](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-gcp/1-9/csb-gcp/reference-gcp-mysql.html)
   - [Tanzu Cloud Service Broker for AWS](https://techdocs.broadcom.com/us/en/vmware-tanzu/platform-services/tanzu-cloud-service-broker-for-aws/1-14/csb-aws/reference-aws-mysql.html)

   and [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)

## Running locally

Be sure to follow the [ActuatorWeb Readme](../ActuatorWeb/README.md) to the point where that application is running before continuing here.

### MySQL

This application depends on a MySQL database. Refer to [Common Tasks](../../../CommonTasks.md#MySQL) to start a server that matches the credentials defined in [appsettings.Development.json](./appsettings.Development.json).

### Administrative Tasks (initialize the database)

In order to demonstrate [Steeltoe Management Tasks](https://docs.steeltoe.io/api/v4/management/tasks.html), the database schema and its contents are managed as administrative tasks.

1. Apply Entity Framework Core database schema migration scripts:

    ```shell
    dotnet run --runtask=MigrateDatabase
    ```

2. Run [`ForecastTask`](./AdminTasks/ForecastTask.cs) to predict weather for the next 7 days:

    ```shell
    dotnet run --runtask=ForecastWeather
    ```

    *Optional* - Add forecast data starting from a specific date and/or number of days:

    ```shell
    dotnet run --runtask=ForecastWeather --fromDate=10/18/2024 --days=30
    ```

> [!NOTE]  
> For the `fromDate` parameter, use values formatted as `yyyy-dd-MM` or `MM/dd/yyyy`.

> [!TIP]
> The task [`ResetTask`](./AdminTasks/ResetTask.cs) can be used to remove all forecast data:
> `dotnet run --runtask=ResetWeather`

3. Use your preferred IDE or `dotnet run` to start the application.

## Running on Tanzu Platform for Cloud Foundry

1. Create a MySQL service instance in an org/space

   ```shell
   cf target -o your-org -s your-space
   cf marketplace
   cf marketplace -e your-offering
   ```

   - When using Tanzu for MySQL on Cloud Foundry:

     ```shell
     cf create-service p.mysql your-plan sampleMySqlService
     ```

   - When using Tanzu Cloud Service Broker for GCP:

     ```shell
     cf create-service csb-google-mysql your-plan sampleMySqlService
     ```

   - When using Tanzu Cloud Service Broker for AWS:

     ```shell
     cf create-service csb-aws-mysql your-plan sampleMySqlService
     ```

1. Wait for the service to become ready (you can check with `cf services`)
1. Run the `cf push` command to deploy from source (you can monitor logs with `cf logs actuator-api-management-sample`)
   - When deploying to Windows, binaries must be built locally before push. Use the following commands instead:

     ```shell
     dotnet publish -r win-x64 --self-contained
     cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
     ```

1. Copy the value of `routes` in the output and open in your browser. The app should start and respond to requests, but the database still needs to be configured with the tasks listed in the next section.

> [!NOTE]  
> The provided manifest will create an app named `actuator-api-management-sample` and attempt to bind it to the MySql service `sampleMySqlService`.

### Running Tasks

Depending on the steps taken to push the application to Cloud Foundry, the commands below may require customization (for example, if the application was not published before pushing to a Linux cell, the path for the command might be `./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi`)

1. Apply Entity Framework Core database migration scripts:

    ```shell
    cf run-task actuator-api-management-sample --command "./Steeltoe.Samples.ActuatorApi runtask=MigrateDatabase" 
    ```

1. Run [`ForecastTask`](./AdminTasks/ForecastTask.cs) to predict weather for the next 7 days:

    ```shell
    cf run-task actuator-api-management-sample --command "./Steeltoe.Samples.ActuatorApi runtask=ForecastWeather" 
    ```

> [!TIP]  
> To remove all forecast data, run [`ResetTask`](./AdminTasks/ResetTask.cs):
>
> ```shell
> cf run-task actuator-api-management-sample --command "./Steeltoe.Samples.ActuatorApi runtask=ResetWeather" 
> ```

---

See the Official [Steeltoe Management Documentation](https://docs.steeltoe.io/api/v4/management/) for more detailed information.
