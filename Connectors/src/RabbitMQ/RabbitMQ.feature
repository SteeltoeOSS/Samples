@cloudfoundry_scaffold
Feature: RabbitMQ Connector
  In order to show you how to use Steeltoe for connecting to RabbitMQ
  You can run some RabbitMQ connection samples

  @net8.0
  @windows
  Scenario: Rabbit Connector (net8.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run: cf push -f manifest-windows.yml -p bin/Release/net8.0/win-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector-sample is started
    When you post "MessageToSend=HEY THERE" to https://rabbitmq-connector-sample/Home/Send
    And you get https://rabbitmq-connector-sample/Home/Receive
    Then you should see "HEY THERE"

  @net8.0
  @linux
  Scenario: Rabbit Connector (net8.0/linux)
    When you run: cf push -f manifest.yml
    And you wait until CloudFoundry app rabbitmq-connector-sample is started
    When you post "MessageToSend=HEY THERE" to https://rabbitmq-connector-sample/Home/Send
    And you get https://rabbitmq-connector-sample/Home/Receive
    Then you should see "HEY THERE"
