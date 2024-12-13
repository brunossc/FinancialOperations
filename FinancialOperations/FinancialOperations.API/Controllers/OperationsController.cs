using FinancialOperations.API.Application.OperationsApp.Dtos;
using FinancialOperations.API.Controllers.Base;
using FinancialOperations.API.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialOperations.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationsController : ControllerAppBase
    {
        private readonly ILogger<OperationsController> _logger;
        private readonly IMediator _mediator;
        private readonly IOperationsService _service;

        public OperationsController(ILogger<OperationsController> logger, IMediator mediator, IOperationsService service)
        {
            _logger = logger;
            _mediator = mediator;
            _service = service;
        }

        [HttpPost("AddCredit")]
        public async Task<IActionResult> Credit(CreditCommandsRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("AddDebit")]
        public async Task<IActionResult> Debit(DebitCommandsRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
