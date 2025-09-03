# Logging Consumer

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))

## Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd LoggingConsumerApplication
dotnet run 

info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\LoggingConsumerApplication\bin\Debug\net6.0
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'LoggingConsumer.Application.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'LoggingConsumer.Application.errors' has 2 subscriber(s).
```

1. Go to your [RabbitMQ broker queues](http://localhost:15672/#/queues). Find the queue named `LoggingConsumer.Application` and click on it (this name is controlled by settings in the `appsettings.json`).
1. Find the `Publish Message` section of the UI and enter a JSON object that represents an instance of the Person class:
   ```json
    { "Name": "Some Person"}
   ```
1. Click `Publish Message`

You will see a log message similar to this:

```bash
Received: Some Person
```
