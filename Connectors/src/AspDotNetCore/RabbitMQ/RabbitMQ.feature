@cloud
Feature: RabbitMQ Connector
    In order to show you how to use Steeltoe for connecting to RabbitMQ
    You can run some RabbitMQ connection samples

    @netcoreapp2.0
    @win10-x64
    Scenario: Rabbit Connector for .Net Core 2.0 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service p-rabbitmq installed
        When you run: cf create-service p-rabbitmq standard myRabbitMQService
        And you wait until CloudFoundry service myRabbitMQService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish
        And you wait until CloudFoundry app rabbitmq-connector is started
        When you post "Message=HEY THERE" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
        And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
        Then you should see "Message=HEY THERE"

    @netcoreapp2.0
    @ubuntu.14.04-x64
    Scenario: Rabbit Connector for .Net Core 2.0 (ubuntu.14.04-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service p-rabbitmq installed
        When you run: cf create-service p-rabbitmq standard myRabbitMQService
        And you wait until CloudFoundry service myRabbitMQService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app rabbitmq-connector is started
        When you post "Message=HEY THERE" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
        And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
        Then you should see "Message=HEY THERE"

    @net461
    @win10-x64
    Scenario: Rabbit Connector for .Net Framework 4.6.1 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service p-rabbitmq installed
        When you run: cf create-service p-rabbitmq standard myRabbitMQService
        And you wait until CloudFoundry service myRabbitMQService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
        And you wait until CloudFoundry app rabbitmq-connector is started
        When you post "Message=HEY THERE" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
        And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
        Then you should see "Message=HEY THERE"
