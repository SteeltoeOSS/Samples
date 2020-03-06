@cloud
Feature: CloudFoundry Configuration
    In order to show you how to use Steeltoe for CloudFoundry configurations
    You can run some CloudFoundry configuration samples

    @netcoreapp3.1
    @win10-x64
    Scenario: CloudFoundry Configuration for netcoreapp3.1/win10-x64
        And you run: dotnet publish -f netcoreapp3.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
        And you wait until CloudFoundry app cloud is started
        When you get https://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"

    @netcoreapp3.1
    @ubuntu.16.04-x64
    Scenario: CloudFoundry Configuration for netcoreapp3.1/ubuntu.16.04-x64
        And you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
        And you wait until CloudFoundry app cloud is started
        When you get https://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"

    @netcoreapp2.1
    @win10-x64
    Scenario: CloudFoundry Configuration for netcoreapp2.1/win10-x64
        And you run: dotnet publish -f netcoreapp2.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
        And you wait until CloudFoundry app cloud is started
        When you get https://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"

    @netcoreapp2.1
    @ubuntu.16.04-x64
    Scenario: CloudFoundry Configuration for netcoreapp2.1/ubuntu.16.04-x64
        And you run: dotnet publish -f netcoreapp2.1 -r ubuntu.16.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.16.04-x64/publish
        And you wait until CloudFoundry app cloud is started
        When you get https://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"

    @net461
    @win10-x64
    Scenario: CloudFoundry Configuration for net461/win10-x64
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
        And you wait until CloudFoundry app cloud is started
        When you get https://cloud.x.y.z/Home/CloudFoundry
        Then you should see "vcap:application:application_name = cloud"
