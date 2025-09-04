@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to SQL Server using Entity Framework Core
  You can run some SQL Server using Entity Framework Core connection samples

  @net8.0
  @linux
  Scenario: SqlServerEFCore (net8.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app sqlserver-efcore-connector-sample is started
    When you get https://sqlserver-efcore-connector-sample/
    Then you should see "1: Test Data 1 - AppDbContext"
    And you should see "2: Test Data 2 - AppDbContext"
