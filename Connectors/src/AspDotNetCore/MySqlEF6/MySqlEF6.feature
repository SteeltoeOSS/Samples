@cloudfoundry_scaffold
Feature: MySqlEF6 Connector
  In order to show you how to use Steeltoe for connecting to MySql using EntityFramework 6
  You can run some MySql using EntityFramework 6 connection samples

  @net461
  @win10-x64
  Scenario: MySqlEF6 Connector (net461/win10-x64)
    When you run: dotnet publish -f net461 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
    And you wait until CloudFoundry app mysqlef6-connector is started
    When you get https://mysqlef6-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Test Data 1"
    And you should see "Key 2 = Test Data 2"

  @netcoreapp3.1
  @win10-x64
  Scenario: MySqlEF6 Connector (netcoreapp3.1/linux-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
    And you wait until CloudFoundry app mysqlef6-connector is started
    When you get https://mysqlef6-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Test Data 1"
    And you should see "Key 2 = Test Data 2"

  @net5.0
  @win10-x64
  Scenario: MySqlEF6 Connector (net5.0/win10-x64)
    When you run: dotnet publish -f net5.0 -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net5.0/win10-x64/publish
    And you wait until CloudFoundry app mysqlef6-connector is started
    When you get https://mysqlef6-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Test Data 1"
    And you should see "Key 2 = Test Data 2"

  @netcoreapp3.1
  @linux-x64
  Scenario: MySqlEF6 Connector (netcoreapp3.1/linux-x64)
    When you run: dotnet publish -f netcoreapp3.1 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/linux-x64/publish
    And you wait until CloudFoundry app mysqlef6-connector is started
    When you get https://mysqlef6-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Test Data 1"
    And you should see "Key 2 = Test Data 2"

  @net5.0
  @linux-x64
  Scenario: MySqlEF6 Connector (net5.0/linux-x64)
    When you run: dotnet publish -f net5.0 -r linux-x64
    And you run in the background: cf push -f manifest.yml -p bin/Debug/net5.0/linux-x64/publish
    And you wait until CloudFoundry app mysqlef6-connector is started
    When you get https://mysqlef6-connector.x.y.z/Home/MySqlData
    Then you should see "Key 1 = Test Data 1"
    And you should see "Key 2 = Test Data 2"
