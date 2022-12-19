@cloudfoundry_scaffold
Feature: PostgreSql Connector
  In order to show you how to use Steeltoe for connecting to PostgreSql
  You can run some PostgreSql connection samples

  @net6.0
  @win10
  Scenario: PostgreSql Connector (net6.0/win10)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app postgres-connector is started
    When you get https://postgres-connector/Home/PostgresData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @net6.0
  @linux
  Scenario: PostgreSql Connector (net6.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app postgres-connector is started
    When you get https://postgres-connector/Home/PostgresData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"
