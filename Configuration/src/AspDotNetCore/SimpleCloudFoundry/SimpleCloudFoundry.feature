@cloud
Feature: Simple CloudFoundry Configuration
    In order to show you how to use Steeltoe for simple CloudFoundry configurations
    You can run some simple CloudFoundry configuration samples

    @netcoreapp2.1
    @win10-x64
    Scenario: Simple CloudFoundry Configuration for .Net Core 2.1 (win10-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service p-config-server installed
        When you run: cf create-service p-config-server standard myConfigServer -c ./config-server.json
        And you wait until CloudFoundry service myConfigServer is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
        And you wait until CloudFoundry app foo is started
        When you get https://foo.x.y.z/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"

    @netcoreapp2.1
    @ubuntu.14.04-x64
    Scenario: Simple CloudFoundry Configuration for .Net Core 2.1 (ubuntu.14.04-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service p-config-server installed
        When you run: cf create-service p-config-server standard myConfigServer -c ./config-server.json
        And you wait until CloudFoundry service myConfigServer is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app foo is started
        When you get https://foo.x.y.z/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"

    @net461
    @win10-x64
    Scenario: Simple CloudFoundry Configuration for .Net Framework 4.6.1 (win10-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service p-config-server installed
        When you run: cf create-service p-config-server standard myConfigServer -c ./config-server.json
        And you wait until CloudFoundry service myConfigServer is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
        And you wait until CloudFoundry app foo is started
        When you get https://foo.x.y.z/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"
