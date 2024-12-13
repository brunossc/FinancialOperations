using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;


namespace FinancialOperations.Consolidator.API.Infrastructure.Repositories
{
    public class OperationRepository : IOperationRepository
    {

        private readonly IMongoCollection<OperationDay> _collection;

        public OperationRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("FinancialOperations_Consolidator");
            _collection = mongoDatabase.GetCollection<OperationDay>("OperationDay");
        }

        public async Task AddAsync(OperationDay operation)
        {
            await _collection.InsertOneAsync(operation);
        }

        public async Task<ICollection<OperationDay>> GetAsync(Expression<Func<OperationDay, bool>> expression)
        {
            var data = await _collection.FindAsync(expression);
            return data.ToList();
        }

        public async Task<long> UpdateAsync(OperationDay entity)
        {
            var resultOpe = await Task.Run(() =>
            {
                var filter = Builders<OperationDay>.Filter.Eq(a => a.Id, entity.Id);
                var update = Builders<OperationDay>.Update.Set(a => a.Total, entity.Total);
                var result = _collection.UpdateOne(filter, update);
                return result.ModifiedCount;
            });

            return resultOpe;
        }
    }
}
