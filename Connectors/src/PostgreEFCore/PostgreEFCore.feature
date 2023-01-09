@cloudfoundry_scaffold
Feature: PostgreEFCore Connector
  In order to show you how to use Steeltoe for connecting to PostgreSql using EntityFramework Core
  You can run some PostgreSql using EntityFramework Core connection samples

  @net6.0
  @win10
  Scenario: PostgreEFCore Connector (net6.0/win10)
    When you run: dotnet publish -r win10-x64 --self-contained
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app postgresefcore-connector is started
    When you get https://postgresefcore-connector/Home/PostgresData
    Then you should see "1: Test Data 1 - EF Core TestContext"
    And you should see "2: Test Data 2 - EF Core TestContext"

  @net6.0
  @linux
  Scenario: PostgreEFCore Connector (net6.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app postgresefcore-connector is started
    When you get https://postgresefcore-connector/Home/PostgresData
    Then you should see "1: Test Data 1 - EF Core TestContext"
    And you should see "2: Test Data 2 - EF Core TestContext"
