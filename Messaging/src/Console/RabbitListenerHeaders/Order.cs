using System;

namespace RabbitListenerHeaders;

[Serializable]
public class Order
{
    public int OrderNumber { get; set; }
    public string OrderType { get; set; }

    public override string ToString()
    {
        return "Order=" + OrderType + ", OrderNumber=" + OrderNumber;
    }
}