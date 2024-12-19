using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using MongoDB.Bson;
using Moq;
using System.Linq.Expressions;

namespace FinancialOperations.Consolidator.API.Test
{
    public class OperationRepositoryTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;

        public OperationRepositoryTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddOperationDay()
        {
            var operationDay = new OperationDay
            {
                Id = ObjectId.GenerateNewId(),
                Day = DateTime.UtcNow,
                Total = 1000m
            };

            _operationRepositoryMock.Setup(repo => repo.AddAsync(operationDay)).Returns(Task.CompletedTask);

            await _operationRepositoryMock.Object.AddAsync(operationDay);

            _operationRepositoryMock.Verify(repo => repo.AddAsync(It.Is<OperationDay>(o => o.Total == 1000m)), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnMatchingOperationDays()
        {
            var operationDay = new OperationDay
            {
                Id = ObjectId.GenerateNewId(),
                Day = DateTime.UtcNow,
                Total = 500m
            };

            var operationDays = new List<OperationDay> { operationDay };

            _operationRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationDay, bool>>>()))
                .ReturnsAsync(operationDays);

            var result = await _operationRepositoryMock.Object.GetAsync(o => o.Total > 100);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(500m, result.First().Total);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOperationDay()
        {
            var operationDay = new OperationDay
            {
                Id = ObjectId.GenerateNewId(),
                Day = DateTime.UtcNow,
                Total = 2000m
            };

            _operationRepositoryMock.Setup(repo => repo.UpdateAsync(operationDay)).ReturnsAsync(1L);

            var updatedCount = await _operationRepositoryMock.Object.UpdateAsync(operationDay);

            Assert.Equal(1L, updatedCount);
        }
    }
}
