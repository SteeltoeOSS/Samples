# Messaging with .NET 6 `WebApplicationBuilder`
This sample shows how to add & configure Steeltoe Messaging with a `WebApplicationBuilder`

This solution contains two projects, one that sends messages and the other that receives messages.

## Common Steps

This sample expects a running RabbitMQ broker instance. You can use a RabbitMQ broker running locally on default port with default user and password.

When no configuration is provided, the Steeltoe Messaging defaults to `guest user` and `localhost` on port `5672`.

If needed, you can add your configuration in `appsettings.json` as follows:

```json
{
  "RabbitMq": {
    "Client": {
      "Uri": "amqp://guest:guest@127.0.0.1"
    }
  }
}

```

Or use the Steeltoe `Connectors` functionality if available in your Cloud platform. 

## Running the projects
In a separate window, go to the root of the `WriteToRabbitMQ` and `MonitorRabbitMQ` project and execute the following command :

```bash
dotnet run
```

In another window, run curl to the `WriteToRabbitMQ` API call:

```bash
curl http://localhost:5000/WriteMessageQueue
```
You should see a response similar to the one below:

![Expected Output](https://github.com/steeltoeoss/samples/blob/main/ExpectedOutput.PNG?raw=true)
