@cloud
Feature: CloudFoundry Configuration Samples
    In order to show you how to use Steeltoe for CloudFoundry configurations
    You can run some CloudFoundry configuration samples

    @netcoreapp2.0
    @win10-x64
    Scenario: CloudFoundry Configuration Sample for .Net Core 2.0 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        When you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish -s windows2012R2
        And you wait until CloudFoundry app cloud is started
        When you get http://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"

    @netcoreapp2.0
    @ubuntu.14.04-x64
    Scenario: CloudFoundry Configuration Sample for .Net Core 2.0 (ubuntu.14.04-x64)
        Given you have .NET Core SDK 2.0 installed
        When you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app cloud is started
        When you get http://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"

    @net461
    @win10-x64
    Scenario: CloudFoundry Configuration Sample for .Net Framework 4.6.1 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        When you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish -s windows2012R2
        And you wait until CloudFoundry app cloud is started
        When you get http://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"
