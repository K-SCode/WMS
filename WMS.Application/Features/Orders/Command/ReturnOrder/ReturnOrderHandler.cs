using Mediator;
using WMS.Application.common.Helper;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.ReturnOrder
{
    public class ReturnOrderHandler(
        IUnitOfWorks unitOfWorks,
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

            if (order!.Status == OrderStatus.Returned)
            {
                return true;
            }

            ChangeOrderStatus.ChangeStatus(order, OrderStatus.Returned);

            await orderRestocking.Restock(order, WarehouseSource.CustomerReturn, cancellationToken);

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
