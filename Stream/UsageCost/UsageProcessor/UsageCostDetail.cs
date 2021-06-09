using System;
using System.Collections.Generic;
using System.Text;

namespace UsageProcessor
{
    public class UsageCostDetail
    {
        public string UserId { get; set; }

        public double CallCost { get; set; }

        public double DataCost { get; set; }

        public override string ToString()
        {
            return $"{{ \"userId\" \"{UserId}\", \"callCost\": \"{CallCost:C}\", \"dataCost\": \"{DataCost:C}\" }}";
        }
    }
}
