@cloudfoundry_scaffold
Feature: RabbitMQ Connector
  In order to show you how to use Steeltoe for connecting to RabbitMQ
  You can run some RabbitMQ connection samples

  @net6.0
  @win10
  Scenario: Rabbit Connector (net6.0/win10)
    When you run: dotnet publish -r win10-x64
    And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net6.0/win10-x64/publish
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE"

  @net6.0
  @linux
  Scenario: Rabbit Connector (net6.0/linux)
    When you run in the background: cf push -f manifest.yml
    And you wait until CloudFoundry app rabbitmq-connector is started
    When you post "Message=HEY THERE" to https://rabbitmq-connector.x.y.z/RabbitMQ/Send
    And you get https://rabbitmq-connector.x.y.z/RabbitMQ/Receive
    Then you should see "Message=HEY THERE"
