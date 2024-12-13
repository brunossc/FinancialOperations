using FinancialOperations.Consolidator.Protos;
using Grpc.Core;

namespace FinancialOperations.Consolidator.API.Services
{
    public class OperationService : OperationServiceProto.OperationServiceProtoBase
    {
        private readonly ILogger<OperationService> _logger;
        public OperationService(ILogger<OperationService> logger)
        {
            _logger = logger;
        }

        public override Task<OperationResponse> CreateOperationAsync(CreateOperationCommand request, ServerCallContext context)
        {
            return Task.FromResult(new OperationResponse
            {
                Id = request.Id
            });
        }

        public override Task<OperationResponse> GetOperationAsync(GetOperationQuery request, ServerCallContext context)
        {
            return Task.FromResult(new OperationResponse
            {
                Id = request.Year
            });
        }
    }
}
