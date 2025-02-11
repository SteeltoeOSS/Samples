@cloudfoundry_scaffold
Feature: MySql Connector
  In order to show you how to use Steeltoe for connecting to MySQL
  You can run some MySQL connection samples

  @net8.0
  @windows
  Scenario: MySql Connector (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app mysql-connector-sample is started
    When you get https://mysql-connector-sample/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"

  @net8.0
  @linux
  Scenario: MySql Connector (net8.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app mysql-connector-sample is started
    When you get https://mysql-connector-sample/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"
