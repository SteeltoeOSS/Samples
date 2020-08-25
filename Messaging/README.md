# Messaging Sample Applications

This repo tree contains sample apps illustrating how to use the Steeltoe Messaging APIs in your .NET applications.

* src/RabbitMQWeb - ASP.NET Core sample app illustrating how to use the Steeltoe Messaging APIs when interacting with a RabbitMQ broker.
* src/Console/GenericHostRabbitListener - .NET Console application using GenericHost and the Steeltoe RabbitListener attribute

Note: There are several other Console application examples in src/Console.

## Building & Running

Each app requires a running RabbitMQ broker to be present on your desktop. Each app can easily be built and run using normal `dotnet run` command line options or within Visual Studio or VSCode.
