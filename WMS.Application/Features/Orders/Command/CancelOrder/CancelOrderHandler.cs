using Mediator;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.CancelOrder
{
    public class CancelOrderHandler(
        IUnitOfWorks unitOfWorks,
        IIncreaseQuantity increaseQuantity,
        ICreateWarehouseTask createWarehouseTask
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
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Cancelled;

            IReadOnlyCollection<OrderItem> items = await unitOfWorks.OrderItems.GetAllByOrderIdAsync(orderId, cancellationToken);

            var xd = await increaseQuantity.IncreaseAsync(items, i => i.Product.Barcode, i => i.Quantity, cancellationToken);

            await createWarehouseTask.CreateTaskAsync(xd, WarehouseSource.OrderCancellation, order.Id, cancellationToken);

            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return true;

        }
    }

    

}
