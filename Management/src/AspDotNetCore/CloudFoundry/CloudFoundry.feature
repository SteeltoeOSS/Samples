@cloud
@mysql
Feature: Cloud Foundry Samples
    In order to show you how to use Steeltoe Management Endpoint
    You can run some Steeltoe Management Endpoint samples

    @netcoreapp3.1
    @win10-x64
    Scenario: CloudFoundry Management for .Net Core 3.1 (win10-x64)
        Given you have at least .Net Core SDK 3.1.100 installed
        And you have CloudFoundry service p.mysql installed
        When you run: cf create-service p.mysql db-small myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp3.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
        And you wait until CloudFoundry app actuator is started
        Then you should be able to access CloudFoundry app actuator management endpoints

    @netcoreapp3.1
    @ubuntu.16.04-x64
    Scenario: CloudFoundry Management for .Net Core 3.1 (ubuntu.16.04-x64)
        Given you have at least .Net Core SDK 3.1.100 installed
        And you have CloudFoundry service p.mysql installed
        When you run: cf create-service p.mysql db-small myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
        And you wait until CloudFoundry app actuator is started
        Then you should be able to access CloudFoundry app actuator management endpoints
