# Basic Stream Listener using WebApplication Builder

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))

## Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd Basic
dotnet run 

info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'BasicSample.Basic.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'BasicSample.Basic.errors' has 2 subscriber(s).
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\Basic\bin\Debug\net6.0\
```

1. Go to your [RabbitMQ broker queues](http://localhost:15672/#/queues). Find the queue named `BasicSample.Basic` and click on it (this name is controlled by settings in the `appsettings.json`).
1. Find the `Publish Message` section of the UI and enter "some message".
1. Click `Publish Message`.

You will see a log message similar to this:

```bash
info: Basic.MyStreamProcessor[0]
      MyStreamProcessor changed input:some message into output:SOME MESSAGE
```
