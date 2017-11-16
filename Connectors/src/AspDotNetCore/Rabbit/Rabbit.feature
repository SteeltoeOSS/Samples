Feature: Rabbit Connector Samples
    In order to show you how to use Steeltoe for connecting to RabbitMQ
    You can run some RabbitMQ connection samples

    @netcoreapp2.0
    @win10-x64
    Scenario: Rabbit Connector Sample for .Net Core 2.0 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you are logged into CloudFoundry
        And you have CloudFoundry service p-rabbitmq installed
        When you run: cf create-service p-rabbitmq standard myRabbitService
        And you wait until CloudFoundry service myRabbitService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish -s windows2012R2
        And you wait until CloudFoundry app rabbit-connector is started
        When you post "Message=HEY THERE" to http://rabbit-connector.x.y.z/Rabbit/Send
        And you get http://rabbit-connector.x.y.z/Rabbit/Receive
        Then you should see "Message=HEY THERE"

    @netcoreapp2.0
    @ubuntu.14.04-x64
    Scenario: Rabbit Connector Sample for .Net Core 2.0 (ubuntu.14.04-x64)
        Given you have .NET Core SDK 2.0 installed
        And you are logged into CloudFoundry
        And you have CloudFoundry service p-rabbitmq installed
        When you run: cf create-service p-rabbitmq standard myRabbitService
        And you wait until CloudFoundry service myRabbitService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app rabbit-connector is started
        When you post "Message=HEY THERE" to http://rabbit-connector.x.y.z/Rabbit/Send
        And you get http://rabbit-connector.x.y.z/Rabbit/Receive
        Then you should see "Message=HEY THERE"
