using FinancialOperations.Consolidator.API.Domain.Model;
using FinancialOperations.Consolidator.API.Domain.Repositories;
using FinancialOperations.Consolidator.API.Domain.Services.Interfaces;

namespace FinancialOperations.Consolidator.API.Domain.Services
{
    public class ProcessOperation : IProcessOperation
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IProcessedOperationRepository _processedOperationRepository;
        private readonly ILogger<ProcessOperation> _logger;

        public ProcessOperation(IOperationRepository operationRepository, IProcessedOperationRepository processedOperationRepository, ILogger<ProcessOperation> logger)
        {
            _operationRepository = operationRepository;
            _processedOperationRepository = processedOperationRepository;
            _logger = logger;
        }

        public async Task Process(ProcessedOperation operation)
        {
            try
            {
                var data = await _operationRepository.GetAsync(o => o.Day.Date == operation.Day.Date);


                if (data is null || data.Count == 0)
                {
                    var operationDay = new OperationDay()
                    {
                        Id = operation.Id,
                        Day = operation.Day,
                        Total = operation.Value
                    };

                    await _operationRepository.AddAsync(operationDay);
                }
                else
                {
                    var operationDay = data.FirstOrDefault();
                    operationDay.Total += operation.IsCredit ? operation.Value : -operation.Value;
                    await _operationRepository.UpdateAsync(operationDay);
                }

                var processedOperation = new ProcessedOperation
                {
                    Id = operation.Id,
                    Day = operation.Day,
                    IsCredit = operation.IsCredit,
                    Value = operation.Value
                };

                await _processedOperationRepository.AddProcessedAsync(processedOperation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
