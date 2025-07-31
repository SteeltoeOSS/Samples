@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to PostgreSQL using Entity Framework Core
  You can run some PostgreSQL using Entity Framework Core connection samples

  @net8.0
  @windows
  Scenario: PostgreSqlEFCore (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app postgresql-efcore-connector-sample is started
    When you get https://postgresql-efcore-connector-sample/
    Then you should see "Test Data 1 - AppDbContext"
    And you should see "Test Data 2 - AppDbContext"
