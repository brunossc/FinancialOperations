﻿using FinancialOperations.API.Application.OperationsApp.Dtos;
using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Model;
using FinancialOperations.MQ.Events;
using MassTransit;
using MediatR;

namespace FinancialOperations.API.Application.OperationsApp.Handlers
{
    public class DebitOperationHandle : IRequestHandler<DebitCommandsRequest, OperationCommandResponse>
    {

        private readonly IOperationsService _operationService;
        private readonly IBus _publisher;

        public DebitOperationHandle(IBus publisher, IOperationsService operationService)
        {
            _publisher = publisher;
            _operationService = operationService;
        }

        public async Task<OperationCommandResponse> Handle(DebitCommandsRequest request, CancellationToken cancellationToken)
        {
            var entity = new Operation()
            {
                Value = request.Value
            };

            var result = await _operationService.AddDebit(entity).ContinueWith
                ((task) =>
                {

                    if (task.IsCompletedSuccessfully)
                    {
                        var msg = new OperationEvent()
                        {
                            Id = task.Result.Id.ToString(),
                            Day = task.Result.CreatedAt,
                            IsCredit = task.Result.IsCredit,
                            Value = task.Result.Value
                        };

                        _publisher.Publish(msg);
                    }

                    return task.Result;
                });

            var response = new OperationCommandResponse()
            {
                IsCredit = result.IsCredit,
                Value = result.Value
            };

            return response;
        }
    }
}
