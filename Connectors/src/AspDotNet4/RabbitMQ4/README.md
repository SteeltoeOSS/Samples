# RabbitMQ Connector Sample App - RabbitMQConnection

This ASP.NET 4.6.1 sample app uses the [Steeltoe RabbitMQ Connector](https://steeltoe.io/docs/steeltoe-connectors/#4-0-rabbitmq) to connect to RabbitMQ on CloudFoundry. 

This sample uses `RabbitMQ.Client` to send and receive messages on the bound RabbitMQ service and Autofac for IoC services.

## Pre-requisites - CloudFoundry

1. Installed Pivotal CloudFoundry
1. Installed Windows support
1. Installed RabbitMQ CloudFoundry service

## Create RabbitMQ Service Instance on CloudFoundry

You must first create an instance of the RabbitMQ service in a org/space.

1. cf target -o myorg -s development
1. cf create-service p-rabbitmq standard myRabbitMQService

## Publish App & Push to CloudFoundry

1. Open Samples\Connectors\src\AspDotNet4\Connectors.sln in Visual Studio 2017.
1. Select RabbitMQ4 project in Solution Explorer.
1. Right-click and select Publish
1. Use `FolderProfile` to publish to `bin/Debug/net461/win10-x64/publish`
1. Use the CF CLI to push the app

```bash
> cd samples\Connectors\src\AspDotNet4\RabbitMQ4
> cf push -p bin\Debug\net461\win10-x64\publish
```

> Note: The provided manifest will create an app named `rabbitmq-connector-4x` and attempt to bind the app to RabbitMQ service `myRabbitMQService`.

## What to expect - CloudFoundry

Use the Cloud Foundry CLI to see the logs as you startup and use the app, with the command `cf logs rabbitmq-connector-4x`

This sample will be available at <http://rabbitmq-connector-4x.[your-cf-apps-domain]/>.

To send a message over RabbitMQ: click "Send" in the menu, enter text and click the Send button.
To receive a RabbitMQ message that you have sent: click "Receive" in the menu, and messages will be retrieved from the queue one at a time (per page view).

---

### See the Official [Steeltoe Service Connectors Documentation](https://steeltoe.io/docs/steeltoe-connectors) for guided tour of the samples and more detailed Connector information