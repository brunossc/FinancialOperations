using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using FinancialOperations.Consolidator.Protos;
using Grpc.Core;

namespace FinancialOperations.Consolidator.API.Services
{
    public class OperationService : OperationServiceProto.OperationServiceProtoBase
    {
        private readonly ILogger<OperationService> _logger;
        private readonly IOperationRepository _repository;

        public OperationService(ILogger<OperationService> logger, IOperationRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public override Task<OperationResponse> CreateOperationAsync(CreateOperationCommand request, ServerCallContext context)
        {
            return Task.FromResult(new OperationResponse
            {
                Id = request.Id
            });
        }

        public override async Task<OperationsDayResponse> GetOperationAsync(GetOperationQuery request, ServerCallContext context)
        {

            var result = await _repository.GetAsync(x=>x.Total > 0);
            var operationsDay = new OperationsDayResponse();
            result.ToList().ForEach(o => {
                operationsDay.OperationsDay.Add(
                    new Protos.OperationDay()
                    {
                        Id = o.Id.ToString(),
                        Value = o.Total.ToString(),
                        Day = o.Day.ToString()                                                  
                    });
                }
            );
            return operationsDay;
        }
    }
}
