@cloud
@mysql
Feature: MySql Connector
    In order to show you how to use Steeltoe for connecting to MySql
    You can run some MySql connection samples

    @netcoreapp3.0
    @win10-x64
    Scenario: MySql Connector for .Net Core 3.0 (win10-x64)
        Given you have at least .NET Core SDK 3.0.100 installed
        And you have CloudFoundry service p.mysql installed
        When you run: cf create-service p.mysql db-small myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp3.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.0/win10-x64/publish
        And you wait until CloudFoundry app mysql-connector is started
        When you get https://mysql-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"

    @netcoreapp3.0
    @ubuntu.16.04-x64
    Scenario: MySql Connector for .Net Core 3.0 (ubuntu.16.04-x64)
        Given you have at least .NET Core SDK 3.0.100 installed
        And you have CloudFoundry service p.mysql installed
        When you run: cf create-service p.mysql db-small myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp3.0 -r ubuntu.16.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.0/ubuntu.16.04-x64/publish
        And you wait until CloudFoundry app mysql-connector is started
        When you get https://mysql-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"
