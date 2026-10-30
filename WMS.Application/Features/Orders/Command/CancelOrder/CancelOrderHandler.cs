using Mediator;
using WMS.Application.Features.Orders.Command.Service;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.CancelOrder
{
    public class CancelOrderHandler(
        IUnitOfWorks unitOfWorks,
        IOrderRestocking orderRestocking
        )
        : ICommandHandler<CancelOrderCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            int orderId = command.OrderId;
            Order? order = await unitOfWorks.Order.GetByIdAsync(orderId,cancellationToken);
            if(order is null)
            {
                return Errors<Order>.ObjectNotExists(orderId);
            }

            if(order.Status == OrderStatus.Cancelled)
            {
                return true;
            }

            ChangeOrderStatus.ChangeStatus(order, OrderStatus.Cancelled);

            await orderRestocking.Restock(order,WarehouseSource.OrderCancellation, cancellationToken);

            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return true;

        }
    }

}
