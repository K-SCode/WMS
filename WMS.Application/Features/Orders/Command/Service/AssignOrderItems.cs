using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public class AssignOrderItems(IUnitOfWork unitOfWorks) : IAssignOrderItems
    {
        public async Task Assing(
            IReadOnlyCollection<OrderItem> request,
            Order order,
            CancellationToken ct)
        {
            IReadOnlyCollection<Task<OrderItem>> tasks = [.. request
                .Select(async p =>
                {
                    var product = await unitOfWorks.Product.GetByIdAsync(p.ProductId, ct);
                    p.Order = order;
                    p.Price = product!.Price;
                    p.UnitQuantity = product.UnitQuantity;
                    return p;
                })];

            List<OrderItem> items = [];
            foreach (var task in tasks)
            {
                items.Add(await task);
            }

            await unitOfWorks.OrderItems.AddRangeAsync(items, ct);
        }
    }
}

