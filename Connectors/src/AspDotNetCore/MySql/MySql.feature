Feature: MySql Connector Samples
    In order to show you how to use Steeltoe for connecting to MySql
    You can run some MySql connection samples

    @netcoreapp2.0
    @win10-x64
    Scenario: MySql Connector Sample for .Net Core 2.0 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you are logged into CloudFoundry
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish -s windows2012R2
        And you wait until CloudFoundry app mysql-connector is started
        When you open http://mysql-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"

    @netcoreapp2.0
    @ubuntu.14.04-x64
    Scenario: MySql Connector Sample for .Net Core 2.0 (ubuntu.14.04-x64)
        Given you have .NET Core SDK 2.0 installed
        And you are logged into CloudFoundry
        And you have CloudFoundry service p-mysql installed
        When you run: cf create-service p-mysql 100mb myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app mysql-connector is started
        When you open http://mysql-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"
