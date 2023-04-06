# RabbitMQ Connector Sample App - RabbitMQConnection

ASP.NET Core sample app illustrating how to use [Steeltoe RabbitMQ Connector](https://docs.steeltoe.io/api/v3/connectors/rabbitmq.html) for connecting to a RabbitMQ service on CloudFoundry.
This sample illustrates using a `RabbitMQ.Client` to send and receive messages on the bound RabbitMQ service.

## General prerequisites

1. Installed .NET Core SDK

## Running locally

1. Started RabbitMQ [docker container](https://github.com/SteeltoeOSS/Samples/blob/main/CommonTasks.md)

## Running on CloudFoundry

1. Installed CloudFoundry (optionally with Windows support)
1. Installed RabbitMQ CloudFoundry service

### Create RabbitMQ Service Instance on CloudFoundry

You must first create an instance of the RabbitMQ service in an org/space.

1. `cf target -o your-org -s your-space`
1. `cf create-service p.rabbitmq single-node myRabbitMQService`

### Publish App & Push to CloudFoundry

1. `cf target -o your-org -s your-space`
1. `cd samples/Connectors/src/RabbitMQ`
1. Push the app
   - When using Windows containers:
     - Publish app to a local directory, specifying the runtime:
       - `dotnet restore --configfile nuget.config`
       - `dotnet publish -r win-x64 --self-contained`
     - Push the app using the appropriate manifest:
       - `cf push -f manifest-windows.yml -p bin/Debug/net6.0/win-x64/publish`
   - Otherwise:
     - Push the app using the appropriate manifest:
       - `cf push -f manifest.yml`

> Note: The provided manifest will create an app named `rabbitmq-connector` and attempt to bind the app to RabbitMQ service `myRabbitMQService`.

### What to expect - CloudFoundry

To see the logs as you startup and use the app: `cf logs rabbitmq-connector`

On a Linux cell, you should see something like this during startup:

```text
2016-08-24T12:22:42.68-0400 [CELL/0]     OUT Creating container
2016-08-24T12:22:43.04-0400 [STG/0]      OUT Successfully destroyed container
2016-08-24T12:22:43.95-0400 [CELL/0]     OUT Successfully created container
2016-08-24T12:22:54.43-0400 [CELL/0]     OUT Starting health monitoring of container
2016-08-24T12:22:56.64-0400 [APP/0]      OUT Project app (.NETCoreApp,Version=v1.0) will be compiled because expected outputs are missing
2016-08-24T12:22:56.67-0400 [APP/0]      OUT Compiling app for .NETCoreApp,Version=v1.0
2016-08-24T12:23:00.57-0400 [APP/0]      OUT Compilation succeeded.
2016-08-24T12:23:00.57-0400 [APP/0]      OUT     0 Warning(s)
2016-08-24T12:23:00.57-0400 [APP/0]      OUT     0 Error(s)
2016-08-24T12:23:00.57-0400 [APP/0]      OUT Time elapsed 00:00:03.9012539
2016-08-24T12:23:01.72-0400 [APP/0]      OUT
2016-08-24T12:23:02.81-0400 [APP/0]      OUT Hosting environment: development
2016-08-24T12:23:02.81-0400 [APP/0]      OUT Content root path: /home/vcap/app
2016-08-24T12:23:02.81-0400 [APP/0]      OUT Now listening on: http://0.0.0.0:8080
2016-08-24T12:23:02.81-0400 [APP/0]      OUT Application started. Press Ctrl+C to shut down.
2016-08-24T12:23:02.89-0400 [CELL/0]     OUT Container became healthy
```

This sample will be available at <http://rabbitmq-connector.[your-cf-apps-domain]/>.

To send a message over RabbitMQ: enter text and click the Send button.
To receive a RabbitMQ message that you have sent: click the Receive button. Messages will be retrieved from the queue one at a time.

---

### See the Official [Steeltoe Service Connectors Documentation](https://docs.steeltoe.io/api/v3/connectors/) for a more in-depth walkthrough of the samples and more detailed information.
