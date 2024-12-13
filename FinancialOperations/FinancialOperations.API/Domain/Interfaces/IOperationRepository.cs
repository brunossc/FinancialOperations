using FinancialOperations.API.Domain.Model;
using System.Linq.Expressions;

namespace FinancialOperations.API.Domain.Interfaces
{
    public interface IOperationRepository
    {
        Task AddAsync(Operation operation);
        Task<ICollection<Operation>> GetAsync(Expression<Func<Operation, bool>> expression);
    }
}