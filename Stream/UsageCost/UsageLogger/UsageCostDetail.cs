namespace UsageLogger
{
    public class UsageCostDetail
    {
        public string UserId { get; set; }

        public double CallCost { get; set; }

        public double DataCost { get; set; }

        public override string ToString()
        {
            return $"{{ \"UserId\" \"{UserId}\", \"CallCost\": \"{CallCost:C}\", \"DataCost\": \"{DataCost:C}\" }}";
        }
    }
}
