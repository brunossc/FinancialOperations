using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Model;
using FinancialOperations.API.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinancialOperations.API.Test
{
    public class OperationsServiceTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<ILogger<OperationsService>> _loggerMock;
        private readonly OperationsService _service;

        public OperationsServiceTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _loggerMock = new Mock<ILogger<OperationsService>>();
            _service = new OperationsService(_operationRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddCredit_SuccessfullyAddsCreditOperation()
        {
            var operation = new Operation { Value = 100 };

            _operationRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Operation>()))
                .Returns(Task.CompletedTask);

            var result = await _service.AddCredit(operation);

            Assert.NotNull(result);
            Assert.True(result.IsCredit);
            Assert.Equal(100, result.Value);
            Assert.NotEqual(default, result.CreatedAt);
        }

        [Fact]
        public async Task AddCredit_HandlesExceptionAndLogsError()
        {
            var operation = new Operation { Value = 100 };

            _operationRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Operation>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _service.AddCredit(operation);

            Assert.NotNull(result);
            Assert.False(result.IsCredit);
            Assert.Equal(0, result.Value);

            _loggerMock.Verify(logger => logger.Log(
            It.Is<LogLevel>(level => level == LogLevel.Error),
            It.Is<EventId>(id => id.Id == 0),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro adicionando uma operação de crédito")),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);

        }

        [Fact]
        public async Task AddDebit_SuccessfullyAddsDebitOperation()
        {
            var operation = new Operation { Value = 50 };

            _operationRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Operation>()))
                .Returns(Task.CompletedTask);

            var result = await _service.AddDebit(operation);

            Assert.NotNull(result);
            Assert.False(result.IsCredit);
            Assert.Equal(50, result.Value);
            Assert.NotEqual(default, result.CreatedAt);
        }

        [Fact]
        public async Task AddDebit_HandlesExceptionAndLogsError()
        {
            var operation = new Operation { Value = 50 };

            _operationRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Operation>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _service.AddDebit(operation);

            Assert.NotNull(result);
            Assert.False(result.IsCredit);
            Assert.Equal(0, result.Value);

            _loggerMock.Verify(logger => logger.Log(
            It.Is<LogLevel>(level => level == LogLevel.Error),
            It.Is<EventId>(id => id.Id == 0),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro adicionando uma operação de débito")),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }
    }
}
