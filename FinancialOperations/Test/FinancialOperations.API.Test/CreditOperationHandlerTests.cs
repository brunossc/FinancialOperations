using FinancialOperations.API.Application.OperationsApp.Dtos;
using FinancialOperations.API.Application.OperationsApp.Handlers;
using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Model;
using FinancialOperations.MQ.Events;
using MassTransit;
using Moq;

namespace FinancialOperations.API.Test
{
    public class CreditOperationHandlerTests
    {
        private readonly Mock<IBus> _publisherMock;
        private readonly Mock<IOperationsService> _operationServiceMock;
        private readonly CreditOperationHandler _handler;

        public CreditOperationHandlerTests()
        {
            _publisherMock = new Mock<IBus>();
            _operationServiceMock = new Mock<IOperationsService>();
            _handler = new CreditOperationHandler(_publisherMock.Object, _operationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_CreditOperation_SuccessfullyPublishesEvent()
        {
            var request = new CreditCommandsRequest { Value = 100 };
            var operation = new Operation
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                Value = 100,
                IsCredit = true,
                CreatedAt = DateTime.UtcNow
            };

            _operationServiceMock
                .Setup(service => service.AddCredit(It.IsAny<Operation>()))
                .ReturnsAsync(operation);

            _publisherMock
                .Setup(bus => bus.Publish(It.IsAny<OperationEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsCredit);
            Assert.Equal(100, result.Value);

            _operationServiceMock.Verify(service => service.AddCredit(It.Is<Operation>(o => o.Value == 100)), Times.Once);

            _publisherMock.Verify(bus => bus.Publish(
                It.Is<OperationEvent>(e => e.Value == 100 && e.IsCredit == true),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
