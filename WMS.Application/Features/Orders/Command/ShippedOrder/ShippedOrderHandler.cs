using Mediator;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.ShippedOrder
{
    public class ShippedOrderHandler(IUnitOfWork unitOfWorks) : ICommandHandler<ShippedOrderCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(ShippedOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await unitOfWorks.Order.GetByIdAsync(command.Id, cancellationToken);
            if (order is null)
            {
                return Errors<Order>.ObjectNotExists(command.Id);
            }

            order.Status = OrderStatus.Shipped;
            await unitOfWorks.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
