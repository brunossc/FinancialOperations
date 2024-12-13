using FinancialOperations.Consolidator.API.Domain.Model;

namespace FinancialOperations.Consolidator.API.Domain.Services.Interfaces
{
    public interface IProcessOperation
    {
        Task Process(ProcessedOperation operation);
    }
}