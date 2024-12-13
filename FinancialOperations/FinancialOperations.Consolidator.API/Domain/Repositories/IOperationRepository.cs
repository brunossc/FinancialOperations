using FinancialOperations.Consolidator.API.Domain.Model;
using System.Linq.Expressions;

namespace FinancialOperations.Consolidator.API.Domain.Repositories
{
    public interface IOperationRepository
    {
        Task AddAsync(OperationDay operation);
        Task<ICollection<OperationDay>> GetAsync(Expression<Func<OperationDay, bool>> expression);
        Task<long> UpdateAsync(OperationDay sampleEntity);
    }
}
