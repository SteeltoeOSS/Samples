@cloudfoundry_scaffold
Feature: PostgreSql Connector
  In order to show you how to use Steeltoe for connecting to PostgreSql
  You can run some PostgreSql connection samples

  @netcoreapp3.1
  @win10-x64
  Scenario: PostgreSql Connector (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app postgres-connector is started
    When you get https://postgres-connector.x.y.z/Home/PostgresData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @netcoreapp3.1
  @linux-x64
  Scenario: PostgreSql Connector (netcoreapp3.1/linux-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish
    And you wait until CloudFoundry app postgres-connector is started
    When you get https://postgres-connector.x.y.z/Home/PostgresData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @net5.0
  @win10-x64
  Scenario: PostgreSql Connector (net5.0/win10-x64)
    When you run: dotnet publish -f net5.0 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net5.0/win10-x64/publish
    And you wait until CloudFoundry app postgres-connector is started
    When you get https://postgres-connector.x.y.z/Home/PostgresData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @net5.0
  @linux-x64
  Scenario: PostgreSql Connector (net5.0/linux-x64)
    When you run: dotnet publish -f net5.0 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/net5.0/linux-x64/publish
    And you wait until CloudFoundry app postgres-connector is started
    When you get https://postgres-connector.x.y.z/Home/PostgresData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"
