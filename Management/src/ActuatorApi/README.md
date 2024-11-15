# Steeltoe Management Sample - Actuators, Administrative Tasks, Metrics and Tracing

ActuatorWeb and ActuatorAPI form an ASP.NET Core-powered sample application that demonstrates how to use several
Steeltoe libraries on their own and with additional tools

In order to avoid duplicating a significant amount of content, the [ActuatorWeb Readme](../ActuatorWeb/README.md)
contains the shared information and this document only holds content unique to ActuatorApi.

## Running locally

Be sure to follow the [ActuatorWeb Readme](../ActuatorWeb/README.md) to the point where that application is running
before continuing here.

### MySQL

This application depends on a MySQL database. Refer to [Common Tasks](../../../CommonTasks.md#MySQL) to start a server
that matches the credentials defined in [appsettings.Development.json](./appsettings.Development.json).

### Administrative Tasks (initialize the database)

In order to demonstrate [Steeltoe Management Tasks](https://docs.steeltoe.io/api/v3/management/tasks.html), the database
schema and its contents are managed as administrative tasks.

1. Apply Entity Framework database schema migration scripts:

    ```shell
    dotnet run --runtask=MigrateDatabase
    ```

1. Run [`ForecastTask`](./AdminTasks/ForecastTask.cs) to predict weather for the next 7 days:

    ```shell
    dotnet run --runtask=ForecastWeather
    ```

    *Optional* - Add forecast data starting from specific date and/or number of days:

    ```shell
    dotnet run --runtask=ForecastWeather --fromDate=10/18/2024 --days=30
    ```

    > [!NOTE]  
    > Use values formatted for invariant culture (`MM/dd/yyyy`) for the `fromDate` parameter.

1. *\*CAUTION\** Run [`ResetTask`](./AdminTasks/ResetTask.cs) to remove all forecast data:

    ```shell
    dotnet run --runtask=ResetWeather
    ```

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

> [!NOTE]  
> The provided manifest will create an app named `actuator-api-management-sample` and attempt to bind to the the app to
> MySql service `sampleMySqlService`.

### Running Tasks

Depending on the steps taken to push the application to Cloud Foundry, the commands below may require customization (for
example, if the application was published before pushing, the path in the command should be shortened to
`./Steeltoe.Samples.ActuatorApi`)

1. Apply Entity Framework database migration scripts:

    ```shell
    cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=MigrateDatabase" 
    ```

1. Run `ForecastTask` to predict weather for the next 7 days:

    ```shell
    cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ForecastWeather" 
    ```

1. *\*CAUTION\** Run `ResetTask` to remove all forecast data:

    ```shell
    cf run-task actuator-api-management-sample --command "./bin/Debug/net8.0/linux-x64/Steeltoe.Samples.ActuatorApi runtask=ResetWeather" 
    ```
