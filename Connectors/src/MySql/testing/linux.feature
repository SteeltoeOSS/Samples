@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to MySQL
  You can run some MySQL connection samples

  @net10.0
  @linux
  Scenario: MySql (net10.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app mysql-connector-sample is started
    When you get https://mysql-connector-sample/
    Then you should see "Row1 Text"
    And you should see "Row2 Text"
