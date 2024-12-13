using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Services.Interfaces;
using FinancialOperations.MQ.Events;
using MassTransit;
using MongoDB.Bson;


namespace FinancialOperations.Consolidator.API.Infrastructure.MQ.Consumers
{
    public class OperationConsumer : IConsumer<OperationEvent>
    {
        private readonly IProcessOperation _processor;
        private readonly ILogger<OperationConsumer> _logger;


        public OperationConsumer(IProcessOperation processor, ILogger<OperationConsumer> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OperationEvent> context)
        {
            var operation = new ProcessedOperation()
            {
                Id = ObjectId.Parse(context.Message.Id),
                Day = context.Message.Day,
                IsCredit = context.Message.IsCredit,
                Value = context.Message.Value,
            };

            _logger.LogInformation($"Received customer: {context.Message.Id}, {context.Message.Day}, {context.Message.IsCredit}, {context.Message.Value}");

            await _processor.Process(operation);
        }
    }
}
