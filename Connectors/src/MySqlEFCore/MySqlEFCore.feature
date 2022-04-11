@cloudfoundry_scaffold
Feature: MySqlEFCore Connector
  In order to show you how to use Steeltoe for connecting to MySql using EntityFramework Core
  You can run some MySql using EntityFramework Core connection samples

  @net6.0
  @win10-x64
  Scenario: MySqlEFCore Connector (net6.0/win10-x64)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
    Then you should see "1: Test Data 1 - EF Core TestContext A"
    And you should see "2: Test Data 2 - EF Core TestContext B"

  @net6.0
  @linux-x64
  Scenario: MySqlEFCore Connector (net6.0/linux-x64)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
    Then you should see "1: Test Data 1 - EF Core TestContext A"
    And you should see "2: Test Data 2 - EF Core TestContext B"
