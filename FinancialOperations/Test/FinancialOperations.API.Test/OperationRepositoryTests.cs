using FinancialOperations.API.Domain.Model;
using FinancialOperations.API.Infrastructure.Repositories;
using Mongo2Go;
using MongoDB.Driver;

namespace FinancialOperations.API.Test
{
    public class OperationRepositoryTests
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoClient _mongoClient;
        private readonly OperationRepository _repository;

        public OperationRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            _mongoClient = new MongoClient(_runner.ConnectionString);
            _repository = new OperationRepository(_mongoClient);
        }

        [Fact]
        public async Task AddAsync_ShouldInsertOperation()
        {
            // Arrange
            var operation = new Operation
            {
                Value = 100,
                IsCredit = true,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _repository.AddAsync(operation);

            var result = await _repository.GetAsync(o => o.Value == 100 && o.IsCredit);

            // Assert
            Assert.Single(result);
            Assert.Equal(100, result.First().Value);
            Assert.True(result.First().IsCredit);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnMatchingOperations()
        {
            // Arrange
            var operations = new List<Operation>
        {
            new Operation { Value = 50, IsCredit = false, CreatedAt = DateTime.UtcNow },
            new Operation { Value = 100, IsCredit = true, CreatedAt = DateTime.UtcNow }
        };

            foreach (var operation in operations)
            {
                await _repository.AddAsync(operation);
            }

            // Act
            var result = await _repository.GetAsync(o => o.IsCredit);

            // Assert
            Assert.Single(result);
            Assert.Equal(100, result.First().Value);
            Assert.True(result.First().IsCredit);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmpty_WhenNoMatchFound()
        {
            // Act
            var result = await _repository.GetAsync(o => o.Value == 999);

            // Assert
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }

}
