using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using FinancialOperations.Consolidator.API.Domain.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using System.Linq.Expressions;


namespace FinancialOperations.Consolidator.API.Test
{
    public class ProcessOperationTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<IProcessedOperationRepository> _processedOperationRepositoryMock;
        private readonly Mock<ILogger<ProcessOperation>> _loggerMock;
        private readonly ProcessOperation _processOperation;

        public ProcessOperationTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _processedOperationRepositoryMock = new Mock<IProcessedOperationRepository>();
            _loggerMock = new Mock<ILogger<ProcessOperation>>();

            _processOperation = new ProcessOperation(
                _operationRepositoryMock.Object,
                _processedOperationRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Process_ShouldAddNewOperationDay_WhenNoExistingData()
        {
            var processedOperation = new ProcessedOperation
            {
                Id = ObjectId.GenerateNewId(),
                Day = DateTime.UtcNow,
                IsCredit = true,
                Value = 100m
            };

            _operationRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationDay, bool>>>()))
                .ReturnsAsync(new List<OperationDay>());

            await _processOperation.Process(processedOperation);

            _operationRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<OperationDay>(o => o.Total == 100m && o.Day.Date == processedOperation.Day.Date)),
                Times.Once
            );
        }

        [Fact]
        public async Task Process_ShouldUpdateExistingOperationDay_WhenDataExists()
        {
            var existingOperationDay = new OperationDay
            {
                Id = ObjectId.GenerateNewId(),
                Day = DateTime.UtcNow,
                Total = 200m
            };

            var processedOperation = new ProcessedOperation
            {
                Id = ObjectId.GenerateNewId(),
                Day = existingOperationDay.Day,
                IsCredit = false,
                Value = 50m
            };

            _operationRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationDay, bool>>>()))
                .ReturnsAsync(new List<OperationDay> { existingOperationDay });

            await _processOperation.Process(processedOperation);

            _operationRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.Is<OperationDay>(o => o.Total == 150m)),
                Times.Once
            );
        }
    }
}
