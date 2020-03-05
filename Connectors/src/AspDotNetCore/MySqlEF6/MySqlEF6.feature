@cloud
@mysql
Feature: MySqlEF6 Connector
    In order to show you how to use Steeltoe for connecting to MySql using EntityFramework 6
    You can run some MySql using EntityFramework 6 connection samples

    @net461
    @win10-x64
    Scenario: MySqlEF6 Connector for net461/win10-x64
        When you run: cf create-service p.mysql db-small myMySqlService
        And you wait until CloudFoundry service myMySqlService is created
        And you run: dotnet restore
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
        And you wait until CloudFoundry app mysqlef6-connector is started
        When you get https://mysqlef6-connector.x.y.z/Home/MySqlData
        Then you should see "Key 1 = Test Data 1"
        And you should see "Key 2 = Test Data 2"
