@cloudfoundry_scaffold
Feature: PostgreSql Connector
  In order to show you how to use Steeltoe for connecting to PostgreSQL
  You can run some PostgreSQL connection samples

  @net8.0
  @linux
  Scenario: PostgreSql Connector (net8.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app postgresql-connector-sample is started
    When you get https://postgresql-connector-sample/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"
