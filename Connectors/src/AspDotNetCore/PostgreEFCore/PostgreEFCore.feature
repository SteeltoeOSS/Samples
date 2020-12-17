@cloudfoundry_scaffold
Feature: PostgreEFCore Connector
  In order to show you how to use Steeltoe for connecting to PostgreSql using EntityFramework Core
  You can run some PostgreSql using EntityFramework Core connection samples

  @netcoreapp3.1
  @win10-x64
  Scenario: PostgreEFCore Connector (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app postgresefcore-connector is started
    When you get https://postgresefcore-connector.x.y.z/Home/PostgresData
    Then you should see "1: Test Data 1 - EF Core TestContext"
    And you should see "2: Test Data 2 - EF Core TestContext"

  @netcoreapp3.1
  @linux-x64
  Scenario: PostgreEFCore Connector (netcoreapp3.1/linux-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish
    And you wait until CloudFoundry app postgresefcore-connector is started
    When you get https://postgresefcore-connector.x.y.z/Home/PostgresData
    Then you should see "1: Test Data 1 - EF Core TestContext"
    And you should see "2: Test Data 2 - EF Core TestContext"

  @netcoreapp2.1
  @win10-x64
  Scenario: PostgreEFCore Connector (netcoreapp2.1/win10-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
    And you wait until CloudFoundry app postgresefcore-connector is started
    When you get https://postgresefcore-connector.x.y.z/Home/PostgresData
    Then you should see "1: Test Data 1 - EF Core TestContext"
    And you should see "2: Test Data 2 - EF Core TestContext"

  @netcoreapp2.1
  @linux-x64
  Scenario: PostgreEFCore Connector (netcoreapp2.1/linux-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/linux-x64/publish
    And you wait until CloudFoundry app postgresefcore-connector is started
    When you get https://postgresefcore-connector.x.y.z/Home/PostgresData
    Then you should see "1: Test Data 1 - EF Core TestContext"
    And you should see "2: Test Data 2 - EF Core TestContext"
