using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.CancelOrder
{
    public class CancelOrderHandler(
        IUnitOfWork unitOfWorks,
        IOrderRestocking orderRestocking
        )
        : ICommandHandler<CancelOrderCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            int orderId = command.OrderId;
            Order? order = await unitOfWorks.Order.GetByIdAsync(orderId, cancellationToken);
            var verifyResult = VerifyOrder(orderId, order);
            if (verifyResult is not null)
            {
                return verifyResult;
            }
            if(order!.Status == OrderStatus.Returned)
            {
                return Errors.CanNotChangeStatus;
            }
            if (order!.Status == OrderStatus.Cancelled)
            {
                return true;
            }

            ChangeOrderStatus.ChangeStatus(order, OrderStatus.Cancelled);
            await RestockItems(order, cancellationToken);

            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return true;

        }

        private async Task RestockItems(Order order, CancellationToken cancellationToken)
        {
            await orderRestocking.Restock(order, WarehouseSource.OrderCancellation, cancellationToken);
        }

        private static Error? VerifyOrder(int orderId, Order? order)
        {
            if (order is null)
            {
                return Errors<Order>.ObjectNotExists(orderId);
            }
            if (order.Status == OrderStatus.Shipped)
            {
                return Errors.CanNotChangeStatus;
            }

            return null;
        }
    }

}
