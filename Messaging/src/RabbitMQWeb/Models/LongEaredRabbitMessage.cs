using System;

namespace RabbitMQWeb.Models;

[Serializable]
public class LongEaredRabbitMessage(string message) : RabbitMessage(message)
{
    public override string ToString()
    {
        return $"long eared rabbit says \"{Message}\"";
    }
}