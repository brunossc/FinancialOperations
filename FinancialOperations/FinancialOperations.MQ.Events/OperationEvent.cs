namespace FinancialOperations.MQ.Events
{
    public class OperationEvent
    {
        public string Id { get; set; } = string.Empty;
        public DateTime Day { get; set; }
        public bool IsCredit { get; set; }
        public decimal Value { get; set; }
    }
}
