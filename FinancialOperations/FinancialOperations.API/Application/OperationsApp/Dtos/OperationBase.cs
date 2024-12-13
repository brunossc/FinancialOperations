using System.ComponentModel.DataAnnotations;

namespace FinancialOperations.API.Application.OperationsApp.Dtos
{
    public abstract class OperationBase
    {
        [Range(0.01, 1000000000, ErrorMessage = "Valor deve ser maior que 0 e menor que 1.000.000.000,00(Hum bilhão)")]
        public decimal Value { get; set; } = 0;
    }
}
