@cloudfoundry_scaffold
Feature: MongoDb Connector
  In order to show you how to use Steeltoe for connecting to MongoDB
  You can run some MongoDB connection samples

  @net8.0
  @linux
  Scenario: MongoDb Connector (net8.0/linux)
    When you push: manifest.yml
    And you wait until CloudFoundry app mongodb-connector-sample is started
    When you get https://mongodb-connector-sample/
    Then you should see "Object1"
    And you should see "Object2"
