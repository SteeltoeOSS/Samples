@cloudfoundry_scaffold
Feature: MySql Connector
  In order to show you how to use Steeltoe for connecting to MySql
  You can run some MySql connection samples

  @netcoreapp3.1
  @win10-x64
  Scenario: MySql Connector (netcoreapp3.1/win10-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @netcoreapp3.1
  @ubuntu.16.04-x64
  Scenario: MySql Connector (netcoreapp3.1/ubuntu.16.04-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @netcoreapp2.1
  @win10-x64
  Scenario: MySql Connector (netcoreapp2.1/win10-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @netcoreapp2.1
  @ubuntu.16.04-x64
  Scenario: MySql Connector (netcoreapp2.1/ubuntu.16.04-x64)
    When you run: dotnet publish -f netcoreapp2.1 -r ubuntu.16.04-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.16.04-x64/publish
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"

  @net461
  @win10-x64
  Scenario: MySql Connector (net461/win10-x64)
    When you run: dotnet publish -f net461 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
    And you wait until CloudFoundry app mysql-connector is started
    When you get https://mysql-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Row1 Text"
    And you should see "Key 2 = Row2 Text"
