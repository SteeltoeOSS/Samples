@cloudfoundry_scaffold
Feature: Connectors
  In order to show you how to use Steeltoe for connecting to MongoDB
  You can run some MongoDB connection samples

  @net10.0
  @windows
  Scenario: MongoDb (net10.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p bin/Release/net10.0/win-x64/publish
    And you wait until CloudFoundry app mongodb-connector-sample is started
    When you get https://mongodb-connector-sample/
    Then you should see "Object1"
    And you should see "Object2"
