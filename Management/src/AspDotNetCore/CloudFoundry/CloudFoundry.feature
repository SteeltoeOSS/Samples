# TODO: how to verify management endpoint

@cloud
Feature: Cloud Foundry Samples
    In order to show you how to use Steeltoe Management Endpoint
    You can run some Steeltoe Management Endpoint samples

    @netcoreapp2.0
    @win10-x64
    Scenario: CloudFoundry Management for .Net Core 2.0 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish
        And you wait until CloudFoundry app actuator is started

    @netcoreapp2.0
    @ubuntu.14.04-x64
    Scenario: CloudFoundry Management for .Net Core 2.0 (ubuntu.14.04-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app actuator is started

    @net461
    @win10-x64
    Scenario: CloudFoundry Management for .Net Framework 4.6.1 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
        And you wait until CloudFoundry app actuator is started
