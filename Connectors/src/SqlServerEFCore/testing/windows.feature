@cloudfoundry_scaffold
Feature: SqlServerEFCore Connector
  In order to show you how to use Steeltoe for connecting to SQL Server using Entity Framework Core
  You can run some SQL Server using Entity Framework Core connection samples

  @net8.0
  @windows
  Scenario: SqlServerEFCore Connector (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app sqlserver-efcore-connector-sample is started
    When you get https://sqlserver-efcore-connector-sample/
    Then you should see "1: Test Data 1 - AppDbContext"
    And you should see "2: Test Data 2 - AppDbContext"
