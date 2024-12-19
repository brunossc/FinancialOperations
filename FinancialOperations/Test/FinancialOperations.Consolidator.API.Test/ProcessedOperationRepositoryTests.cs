using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace FinancialOperations.Consolidator.API.Test
{
    public class ProcessedOperationRepositoryTests
    {
        private readonly Mock<IMongoCollection<ProcessedOperation>> _collectionMock;
        private readonly Mock<IMongoClient> _mongoClientMock;
        private readonly ProcessedOperationRepository _repository;

        public ProcessedOperationRepositoryTests()
        {
            _collectionMock = new Mock<IMongoCollection<ProcessedOperation>>();
            _mongoClientMock = new Mock<IMongoClient>();

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase
                .Setup(db => db.GetCollection<ProcessedOperation>("ProcessedOperations", null))
                .Returns(_collectionMock.Object);

            _mongoClientMock
                .Setup(client => client.GetDatabase("FinancialOperations_Consolidator", null))
                .Returns(mockDatabase.Object);

            _repository = new ProcessedOperationRepository(_mongoClientMock.Object);
        }

        [Fact]
        public async Task AddProcessedAsync_ShouldInsertProcessedOperation()
        {
            var processedOperation = new ProcessedOperation
            {
                Id = ObjectId.GenerateNewId(),
                Day = DateTime.UtcNow,
                IsCredit = true,
                Value = 500m
            };

            _collectionMock
                .Setup(coll => coll.InsertOneAsync(processedOperation, null, default))
                .Returns(Task.CompletedTask);

            await _repository.AddProcessedAsync(processedOperation);
        }
    }
}
