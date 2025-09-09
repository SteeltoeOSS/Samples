using System;

namespace RabbitMQWeb.Models;

[Serializable]
public class RabbitMessage
{
    public RabbitMessage(string message)
    {
        Message = message;
    }

    public string Message { get; }

    public override string ToString()
    {
        return $"rabbit says \"{Message}\"";
    }
}