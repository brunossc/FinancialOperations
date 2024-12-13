using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using MongoDB.Driver;


namespace FinancialOperations.Consolidator.API.Infrastructure.Repositories
{
    public class ProcessedOperationRepository : IProcessedOperationRepository
    {
        private readonly IMongoCollection<ProcessedOperation> _collection;

        public ProcessedOperationRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("FinancialOperations_Consolidator");
            _collection = mongoDatabase.GetCollection<ProcessedOperation>("ProcessedOperations");
        }

        public async Task AddProcessedAsync(ProcessedOperation processedOperation)
        {
            await _collection.InsertOneAsync(processedOperation);
        }
    }
}
