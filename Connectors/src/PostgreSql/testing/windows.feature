@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to PostgreSQL
  You can run some PostgreSQL connection samples

  @net8.0
  @windows
  Scenario: PostgreSql (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app postgresql-connector-sample is started
    When you get https://postgresql-connector-sample/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"
