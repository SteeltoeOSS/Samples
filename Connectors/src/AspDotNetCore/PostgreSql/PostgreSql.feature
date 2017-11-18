@cloud
Feature: PostgreSql Connector Samples
    In order to show you how to use Steeltoe for connecting to PostgreSql
    You can run some PostgreSql connection samples

    @netcoreapp2.0
    @win10-x64
    Scenario: PostgreSql Connector Sample for .Net Core 2.0 (win10-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service EDB-Shared-PostgreSQL installed
        When you run: cf create-service EDB-Shared-PostgreSQL "Basic PostgreSQL Plan" myPostgres
        And you wait until CloudFoundry service myPostgres is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.0/win10-x64/publish -s windows2012R2
        And you wait until CloudFoundry app postgres-connector is started
        When you get https://postgres-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"

    @netcoreapp2.0
    @ubuntu.14.04-x64
    Scenario: PostgreSql Connector Sample for .Net Core 2.0 (ubuntu.14.04-x64)
        Given you have .NET Core SDK 2.0 installed
        And you have CloudFoundry service EDB-Shared-PostgreSQL installed
        When you run: cf create-service EDB-Shared-PostgreSQL "Basic PostgreSQL Plan" myPostgres
        And you wait until CloudFoundry service myPostgres is created
        And you run: dotnet restore --configfile nuget.config
        And you run: dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app postgres-connector is started
        When you get https://postgres-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"
