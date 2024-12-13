namespace FinancialOperations.API.Domain.Model
{
    public class Operation : ModelBase
    {
        public decimal Value { get; set; }
        public bool IsCredit { get; set; }
    }
}
