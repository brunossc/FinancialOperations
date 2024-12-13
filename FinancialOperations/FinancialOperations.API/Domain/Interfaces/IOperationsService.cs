using FinancialOperations.API.Domain.Model;

namespace FinancialOperations.API.Domain.Interfaces
{
    public interface IOperationsService
    {
        Task<Operation> AddCredit(Operation operation);
        Task<Operation> AddDebit(Operation operation);
    }
}
