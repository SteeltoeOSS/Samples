@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to MySQL
  You can run some MySQL connection samples

  @net10.0
  @windows
  Scenario: MySql (net10.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net10.0/win-x64/publish
    And you wait until CloudFoundry app mysql-connector-sample is started
    When you get https://mysql-connector-sample/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"
