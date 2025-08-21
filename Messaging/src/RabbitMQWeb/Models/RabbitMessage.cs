using System;

namespace RabbitMQWeb.Models;

[Serializable]
public class RabbitMessage(string message)
{
    public string Message { get; } = message;

    public override string ToString()
    {
        return $"rabbit says \"{Message}\"";
    }
}