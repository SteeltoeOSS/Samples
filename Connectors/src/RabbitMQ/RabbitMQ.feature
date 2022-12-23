@cloudfoundry_scaffold
Feature: RabbitMQ Connector
  In order to show you how to use Steeltoe for connecting to RabbitMQ
  You can run some RabbitMQ connection samples

  @net7.0
  @windows
  Scenario: Rabbit Connector (net7.0/windows)
    When you run: dotnet publish -r win-x64 --self-contained
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net7.0/win-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "MessageToSend=HEY THERE" to https://rabbitmq-connector/Home/Send
    And you get https://rabbitmq-connector/Home/Receive
    Then you should see "HEY THERE"

  @net7.0
  @linux
  Scenario: Rabbit Connector (net7.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "MessageToSend=HEY THERE" to https://rabbitmq-connector/Home/Send
    And you get https://rabbitmq-connector/Home/Receive
    Then you should see "HEY THERE"
