namespace RabbitMQ.Models;

public sealed class RabbitViewModel
{
    public string? MessageToSend { get; set; }
    public RabbitSendStatus? SendStatus { get; set; }
    public string? MessageReceived { get; set; }
}
