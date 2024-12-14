using FinancialOperations.Consolidator.API.Services;
using FinancialOperations.Consolidator.Protos;
using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinancialOperations.Consolidator.API.Test
{

    public class OperationServiceTests
    {
        private readonly Mock<ILogger<OperationService>> _loggerMock;
        private readonly Mock<IOperationRepository> _repositoryMock;
        private readonly OperationService _service;

        public OperationServiceTests()
        {
            _loggerMock = new Mock<ILogger<OperationService>>();
            _repositoryMock = new Mock<IOperationRepository>();
            _service = new OperationService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateOperationAsync_ShouldReturnOperationResponseWithCorrectId()
        {
            var request = new CreateOperationCommand
            {
                Id = "123"
            };

            var response = await _service.CreateOperationAsync(request, null);


            Assert.NotNull(response);
            Assert.Equal("123", response.Id);
        }

        [Fact]
        public async Task GetOperationAsync_ShouldReturnOperationDaysResponseWithCorrectData()
        {
            // Arrange
            var operations = new List<Domain.Model.OperationDay>
        {
            new Domain.Model.OperationDay
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                Day = new DateTime(2024, 12, 13),
                Total = 1000m
            },
            new Domain.Model.OperationDay
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                Day = new DateTime(2024, 12, 14),
                Total = 2000m
            }
        };

            _repositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Model.OperationDay, bool>>>()))
                .ReturnsAsync(operations);

            var request = new GetOperationQuery();

            var response = await _service.GetOperationAsync(request, null);

            Assert.NotNull(response);
            Assert.Equal(2, response.OperationsDay.Count);

            Assert.Contains(response.OperationsDay, o => o.Id == operations[0].Id.ToString() && o.Value == "1000" && o.Day == operations[0].Day.ToString());
            Assert.Contains(response.OperationsDay, o => o.Id == operations[1].Id.ToString() && o.Value == "2000" && o.Day == operations[1].Day.ToString());
        }
    }


}