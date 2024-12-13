using FinancialOperations.Consolidator.API.Domain.Model;

namespace FinancialOperations.Consolidator.API.Domain.Repositories
{
    public interface IProcessedOperationRepository
    {
        Task AddProcessedAsync(ProcessedOperation processedOperation);
    }
}
