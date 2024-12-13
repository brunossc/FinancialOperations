using FinancialOperations.API.Application.OperationsApp.Dtos;
using FinancialOperations.API.Controllers;
using FinancialOperations.API.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinancialOperations.API.Test
{

    public class OperationsControllerTests
    {
        private readonly Mock<ILogger<OperationsController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IOperationsService> _serviceMock;
        private readonly OperationsController _controller;

        public OperationsControllerTests()
        {
            _loggerMock = new Mock<ILogger<OperationsController>>();
            _mediatorMock = new Mock<IMediator>();
            _serviceMock = new Mock<IOperationsService>();
            _controller = new OperationsController(_loggerMock.Object, _mediatorMock.Object, _serviceMock.Object);
        }

        [Fact]
        public async Task Credit_ReturnsOkResult_WithExpectedResponse()
        {

            var request = new CreditCommandsRequest { Value = 100 };
            var expectedResponse = new OperationCommandResponse { IsCredit = true, Value = 100 };

            _mediatorMock
                .Setup(m => m.Send(request, default))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Credit(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationCommandResponse>(okResult.Value);

            Assert.Equal(expectedResponse.IsCredit, response.IsCredit);
            Assert.Equal(expectedResponse.Value, response.Value);

            _mediatorMock.Verify(m => m.Send(request, default), Times.Once);
        }

        [Fact]
        public async Task Debit_ReturnsOkResult_WithExpectedResponse()
        {
            var request = new DebitCommandsRequest { Value = 50 };
            var expectedResponse = new OperationCommandResponse { IsCredit = false, Value = 50 };

            _mediatorMock
                .Setup(m => m.Send(request, default))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Debit(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OperationCommandResponse>(okResult.Value);

            Assert.Equal(expectedResponse.IsCredit, response.IsCredit);
            Assert.Equal(expectedResponse.Value, response.Value);

            _mediatorMock.Verify(m => m.Send(request, default), Times.Once);
        }
    }

}