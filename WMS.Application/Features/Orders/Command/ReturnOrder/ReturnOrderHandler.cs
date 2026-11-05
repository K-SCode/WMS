using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.ReturnOrder
{
    public class ReturnOrderHandler(
        IUnitOfWork unitOfWorks,
        IOrderRestocking orderRestocking)
        : ICommandHandler<ReturnOrderCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(
            ReturnOrderCommand command,
            CancellationToken cancellationToken)
        {
            int orderId = command.Id;
            Order? order = await unitOfWorks.Order.GetByIdAsync(orderId, cancellationToken);
            var verifyResult = VerifyData(orderId, order);
            if (verifyResult is not null)
            {
                return verifyResult;
            }

            if(order!.Status == OrderStatus.Cancelled)
            {
                return Errors.CanNotChangeStatus;
            }

            if (order!.Status == OrderStatus.Returned)
            {
                return true;
            }

            ChangeOrderStatus.ChangeStatus(order, OrderStatus.Returned);

            var restockingResult = await orderRestocking.Restock(order, WarehouseSource.CustomerReturn, cancellationToken);
            if(!restockingResult)
            {
                return false;
            }
            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static Error? VerifyData(int orderId, Order? order)
        {
            if (order is null)
            {
                return Errors<Order>.ObjectNotExists(orderId);
            }

            if (order.Status != OrderStatus.Shipped)
            {
                return Errors.CanNotChangeStatus;
            }

            return null;
        }
    }
}
