@cloudfoundry_scaffold
Feature: Api
  In order to show you how to use Steeltoe Actuators
  You can run some Steeltoe Management samples

  @net8.0
  @windows
  Scenario: CloudFoundry Management (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you push: manifest-windows.yml with args: -p ./bin/Release/net8.0/win-x64/publish --random-route
    And you wait until CloudFoundry app actuator-api-management-sample is started
    Then you should be able to access CloudFoundry app actuator-api-management-sample management endpoints
    When you run: cf run-task actuator-api-management-sample --command "./Steeltoe.Samples.ActuatorApi runtask=MigrateDatabase --name MigrateDatabase"
    And you wait until CloudFoundry task MigrateDatabase for actuator-api-management-sample is successful
    When you run: cf run-task actuator-api-management-sample --command "./Steeltoe.Samples.ActuatorApi runtask=ForecastWeather --name ForecastWeather"
    And you wait until CloudFoundry task ForecastWeather for actuator-api-management-sample is successful
    When you get https://actuator-api-management-sample/weatherforecast
    Then the response should contain "temperatureF"
