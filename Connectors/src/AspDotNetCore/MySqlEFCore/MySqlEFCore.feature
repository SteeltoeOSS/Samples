@cloud
Feature: MySqlEFCore Connector
    In order to show you how to use Steeltoe for connecting to MySql using EntityFramework Core
    You can run some MySql using EntityFramework Core connection samples

    @netcoreapp2.1
    @win10-x64
    Scenario: MySqlEFCore Connector for .Net Core 2.1 (win10-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp2.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
        And you wait until CloudFoundry app mysqlefcore-connector is started
        When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
        Then you should see "1: Test Data 1 - EF Core TestContext A"
        And you should see "2: Test Data 2 - EF Core TestContext B"

    @netcoreapp2.1
    @ubuntu.14.04-x64
    Scenario: MySqlEFCore Connector for .Net Core 2.1 (ubuntu.14.04-x64)
        Given you have at least .NET Core SDK 2.1.300 installed
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app mysqlefcore-connector is started
        When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
        Then you should see "1: Test Data 1 - EF Core TestContext A"
        And you should see "2: Test Data 2 - EF Core TestContext B"
