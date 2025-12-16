@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to MySQL using Entity Framework Core
  You can run some MySQL using Entity Framework Core connection samples

  @net10.0
  @linux
  Scenario: MySqlEFCore (net10.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app mysql-efcore-connector-sample is started
    When you get https://mysql-efcore-connector-sample/
    Then you should see "Test Data 1 - AppDbContext"
    And you should see "Test Data 2 - AppDbContext"
