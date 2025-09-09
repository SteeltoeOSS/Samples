using System;

namespace RabbitMQWeb.Models;

[Serializable]
public class LongEaredRabbitMessage : RabbitMessage
{
    public LongEaredRabbitMessage(string message) : base(message)
    {
    }

    public override string ToString()
    {
        return $"long eared rabbit says \"{Message}\"";
    }
}