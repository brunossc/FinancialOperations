using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Model;
using MongoDB.Driver;
using System.Linq.Expressions;


namespace FinancialOperations.API.Infrastructure.Repositories
{
    public class OperationRepository : IOperationRepository
    {

        private readonly IMongoCollection<Operation> _collection;

        public OperationRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("FinancialOperations");
            _collection = mongoDatabase.GetCollection<Operation>("Operation");
        }

        public async Task AddAsync(Operation operation)
        {
            await _collection.InsertOneAsync(operation);
        }

        public async Task<ICollection<Operation>> GetAsync(Expression<Func<Operation, bool>> expression)
        {
            var data = await _collection.FindAsync(expression);
            return data.ToList();
        }
    }
}
