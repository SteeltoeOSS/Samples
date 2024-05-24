@cloudfoundry_scaffold
Feature: MySqlEFCore Connector
  In order to show you how to use Steeltoe for connecting to MySQL using Entity Framework Core
  You can run some MySQL using Entity Framework Core connection samples

  @net8.0
  @windows
  Scenario: MySqlEFCore Connector (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector/
    Then you should see "Test Data 1 - AppDbContext"
    And you should see "Test Data 2 - AppDbContext"

  @net8.0
  @linux
  Scenario: MySqlEFCore Connector (net8.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector/
    Then you should see "Test Data 1 - AppDbContext"
    And you should see "Test Data 2 - AppDbContext"
