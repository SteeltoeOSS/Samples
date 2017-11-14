Feature: Simple Configuration Examples
    In order to show you how to use Steeltoe for simple CloudFoundry configurations
    You can run a some simple CloudFoundry configuration examples

    @netcoreapp2.0
    Scenario Outline: Simple CloudFoundry Configuration Example for .Net Core 2.0
        Given you have .NET Core SDK 2.0 installed
        And you are logged into CloudFoundry
        And you have Spring Cloud Services installed
        When you run: cf create-service p-config-server standard myConfigServer -c ./config-server.json
        And you wait until CloudFoundry service myConfigServer is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r <runtime>
        And you run in the background: cf push -f <manifest> -p bin/Debug/netcoreapp2.0/<runtime>/publish -s <stack>
        And you wait until CloudFoundry app foo is started
        When you open http://foo.x.y.z/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"

        Examples:
            | runtime          | manifest             | stack         |
            | ubuntu.14.04-x64 | manifest.yml         | cflinuxfs2    |
            | win10-x64        | manifest-windows.yml | windows2012R2 |

    @net461
    Scenario Outline: Simple CloudFoundry Configuration Example for .Net Framework 4.6.1
        Given you have .NET Core SDK 2.0 installed
        And you are logged into CloudFoundry
        And you have Spring Cloud Services installed
        When you run: cf create-service p-config-server standard myConfigServer -c ./config-server.json
        And you wait until CloudFoundry service myConfigServer is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f net461 -r <runtime>
        And you run in the background: cf push -f <manifest> -p bin/Debug/net461/<runtime>/publish -s <stack>
        And you wait until CloudFoundry app foo is started
        When you open http://foo.x.y.z/Home/ConfigServerSettings
        Then you should see "spring:cloud:config:name = foo"

        Examples:
            | runtime          | manifest             | stack         |
            | win10-x64        | manifest-windows.yml | windows2012R2 |
