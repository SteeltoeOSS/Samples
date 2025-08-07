# RabbitMQWeb - ASP.NET Core Sample Applicaiton

ASP.NET Core sample app illustrating how to use [RabbitMQ](https://https://www.rabbitmq.com/) in an ASP.NET Core web application.

## Pre-requisites - Running Local

This sample assumes that there is a running RabbitMQ broker on your machine. To make this happen follow the RabbitMQ [Get Started Instructions](https://www.rabbitmq.com/#getstarted)

## Building & Running - Local

1. Clone this repo. (i.e. git clone <https://github.com/SteeltoeOSS/Samples>)
1. cd samples/Messaging/src/AspDotNetCore/RabbitMQWeb
1. dotnet restore --configfile nuget.config
1. dotnet run

## What to expect - Local

After building and running the app, you should see something like the following:

```bash
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\workspace\Samples\Messaging\src\AspDotNetCore\RabbitMQWeb
```

At this point the application is up and running and ready for usage.  Have a look at the `RabbitController.cs` and `RabbitListener.cs` for details on how to use the sample.