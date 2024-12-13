using MediatR;

namespace FinancialOperations.API.Application.OperationsApp.Dtos
{
    public class DebitCommandsRequest : OperationBase, IRequest<OperationCommandResponse>
    {
    }
}
