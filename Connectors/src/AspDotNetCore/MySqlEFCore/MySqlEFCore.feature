@cloudfoundry_scaffold
Feature: MySqlEFCore Connector
  In order to show you how to use Steeltoe for connecting to MySql using EntityFramework Core
  You can run some MySql using EntityFramework Core connection samples

  @netcoreapp3.1
  @win10-x64
  Scenario: MySqlEFCore Connector (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
    Then you should see "1: Test Data 1 - EF Core TestContext A"
    And you should see "2: Test Data 2 - EF Core TestContext B"

  @netcoreapp3.1
  @ubuntu.16.04-x64
  Scenario: MySqlEFCore Connector (netcoreapp3.1/ubuntu.16.04-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
    Then you should see "1: Test Data 1 - EF Core TestContext A"
    And you should see "2: Test Data 2 - EF Core TestContext B"

  @netcoreapp2.1
  @win10-x64
  Scenario: MySqlEFCore Connector (netcoreapp2.1/win10-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
    Then you should see "1: Test Data 1 - EF Core TestContext A"
    And you should see "2: Test Data 2 - EF Core TestContext B"

  @netcoreapp2.1
  @ubuntu.16.04-x64
  Scenario: MySqlEFCore Connector (netcoreapp2.1/ubuntu.16.04-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app mysqlefcore-connector is started
    When you get https://mysqlefcore-connector.x.y.z/Home/MySqlData
    Then you should see "1: Test Data 1 - EF Core TestContext A"
    And you should see "2: Test Data 2 - EF Core TestContext B"
