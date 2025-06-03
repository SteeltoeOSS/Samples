@cloudfoundry_scaffold
Feature: PostgreSqlEFCore Connector
  In order to show you how to use Steeltoe for connecting to PostgreSQL using Entity Framework Core
  You can run some PostgreSQL using Entity Framework Core connection samples

  @net8.0
  @linux
  Scenario: PostgreSqlEFCore Connector (net8.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app postgresql-efcore-connector-sample is started
    When you get https://postgresql-efcore-connector-sample/
    Then you should see "Test Data 1 - AppDbContext"
    And you should see "Test Data 2 - AppDbContext"
