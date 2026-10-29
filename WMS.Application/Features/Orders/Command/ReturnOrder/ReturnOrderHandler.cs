using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Command.ReturnOrder
{
    public class ReturnOrderHandler : ICommandHandler<ReturnOrderCommand, Result<bool>>
    {
        public ValueTask<Result<bool>> Handle(
            ReturnOrderCommand command,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
