using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGenericHost
{
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
}
