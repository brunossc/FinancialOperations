using FinancialOperations.API.Application.OperationsApp.Dtos;
using FinancialOperations.API.Application.OperationsApp.Handlers;
using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Model;
using FinancialOperations.MQ.Events;
using MassTransit;
using Moq;

namespace FinancialOperations.API.Test
{
    public class DebitOperationHandlerTests
    {
        private readonly Mock<IBus> _publisherMock;
        private readonly Mock<IOperationsService> _operationServiceMock;
        private readonly DebitOperationHandle _handler;

        public DebitOperationHandlerTests()
        {
            _publisherMock = new Mock<IBus>();
            _operationServiceMock = new Mock<IOperationsService>();
            _handler = new DebitOperationHandle(_publisherMock.Object, _operationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DebitOperation_SuccessfullyPublishesEvent()
        {
            // Arrange
            var request = new DebitCommandsRequest { Value = 50 };
            var operation = new Operation
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                Value = 50,
                IsCredit = false,
                CreatedAt = DateTime.UtcNow
            };

            _operationServiceMock
                .Setup(service => service.AddDebit(It.IsAny<Operation>()))
                .ReturnsAsync(operation);

            _publisherMock
                .Setup(bus => bus.Publish(It.IsAny<OperationEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsCredit);
            Assert.Equal(50, result.Value);

            _operationServiceMock.Verify(service => service.AddDebit(It.Is<Operation>(o => o.Value == 50)), Times.Once);

            _publisherMock.Verify(bus => bus.Publish(
                It.Is<OperationEvent>(e => e.Value == 50 && e.IsCredit == false),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}

