using MediatR;

namespace FinancialOperations.API.Application.OperationsApp.Dtos
{
    public class CreditCommandsRequest : OperationBase, IRequest<OperationCommandResponse>
    {
    }
}
