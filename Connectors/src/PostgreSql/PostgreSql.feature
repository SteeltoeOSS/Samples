@cloudfoundry_scaffold
Feature: PostgreSql Connector
  In order to show you how to use Steeltoe for connecting to PostgreSQL
  You can run some PostgreSQL connection samples

  @net8.0
  @windows
  Scenario: PostgreSql Connector (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app postgresql-connector is started
    When you get https://postgresql-connector/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"

  @net8.0
  @linux
  Scenario: PostgreSql Connector (net8.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app postgresql-connector is started
    When you get https://postgresql-connector/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"
