using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    internal class OrderRestocking(
        IUnitOfWorks unitOfWorks,
        IIncreaseQuantity increaseQuantity,
        ICreateWarehouseTask createWarehouseTask) : IOrderRestocking
    {
        public async Task Restock(Order order, WarehouseSource source, CancellationToken ct)
        {
            IReadOnlyCollection<OrderItem> items = await unitOfWorks.OrderItems.GetAllByOrderIdAsync(order.Id, ct);

            var xd = await increaseQuantity.IncreaseAsync(items, i => i.Product.Barcode, i => i.Quantity, ct);

            await createWarehouseTask.CreateTaskAsync(xd, source, order.Id, ct);
        }
    }
}
