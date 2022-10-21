@cloudfoundry_scaffold
Feature: MySql Connector
  In order to show you how to use Steeltoe for connecting to MySql
  You can run some MySql connection samples

  @net6.0
  @win10
  Scenario: MySql Connector (net6.0/win10)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @net6.0
  @linux
  Scenario: MySql Connector (net6.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"
