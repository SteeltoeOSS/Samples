@cloud
@postgresql
Feature: PostgreSql Connector
    In order to show you how to use Steeltoe for connecting to PostgreSql
    You can run some PostgreSql connection samples

    @netcoreapp3.1
    @win10-x64
    Scenario: PostgreSql Connector for netcoreapp3.1/win10-x64
        When you run: cf create-service postgresql-10-odb standalone myPostgres -c '{"db_name":"postgresample", "db_username":"steeltoe", "owner_name":"Steeltoe Demo", "owner_email":"demo@steeltoe.io"}'
        And you wait until CloudFoundry service myPostgres is created
        And you run: dotnet publish -f netcoreapp3.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp3.1/win10-x64/publish
        And you wait until CloudFoundry app postgres-connector is started
        When you get https://postgres-connector.x.y.z/Home/PostgresData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"

    @netcoreapp3.1
    @ubuntu.16.04-x64
    Scenario: PostgreSql Connector for netcoreapp3.1/ubuntu.16.04-x64
        When you run: cf create-service postgresql-10-odb standalone myPostgres -c '{"db_name":"postgresample", "db_username":"steeltoe", "owner_name":"Steeltoe Demo", "owner_email":"demo@steeltoe.io"}'
        And you wait until CloudFoundry service myPostgres is created
        And you run: dotnet publish -f netcoreapp3.1 -r ubuntu.16.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp3.1/ubuntu.16.04-x64/publish
        And you wait until CloudFoundry app postgres-connector is started
        When you get https://postgres-connector.x.y.z/Home/PostgresData
        Then you should see "Key 1 = Row1 Text"
        And you should see "Key 2 = Row2 Text"
