using FinancialOperations.API.Domain.Interfaces;
using FinancialOperations.API.Domain.Model;

namespace FinancialOperations.API.Domain.Services
{
    public class OperationsService : IOperationsService
    {

        private readonly IOperationRepository _operationRepository;
        private readonly ILogger<OperationsService> _logger;

        public OperationsService(IOperationRepository repo, ILogger<OperationsService> logger)
        {
            _operationRepository = repo;
            _logger = logger;
        }

        public async Task<Operation> AddCredit(Operation operation)
        {
            try
            {
                operation.IsCredit = true;
                return await this.Add(operation);
            }
            catch (Exception ex)
            {
                var param = new object[] { operation };
                _logger.LogError(ex, "Erro adicionando uma operação de crédito", param);
                return new Operation();
            }
        }

        public async Task<Operation> AddDebit(Operation operation)
        {
            try
            {
                return await this.Add(operation);
            }
            catch (Exception ex)
            {
                var param = new object[] { operation };
                _logger.LogError(ex, "Erro adicionando uma operação de débito", param);
                return new Operation();
            }
        }

        private async Task<Operation> Add(Operation operation)
        {
            operation.CreatedAt = DateTime.UtcNow;
            await _operationRepository.AddAsync(operation);
            return operation;
        }
    }
}

