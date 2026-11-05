using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Orders;
using WMS.Core.Entities;

namespace WMS.Application.Features.Orders.Command.Service
{
    public class AssignOrderItems(IUnitOfWork unitOfWorks) : IAssignOrderItems
    {
        public async Task<bool> Assing(
            IReadOnlyCollection<OrderItem> request,
            Order order,
            CancellationToken ct)
        {
            foreach (var p in request)
            {
                var product = await GetProductByIdAsync(p, ct);
                if (product is null)
                {
                    return false;
                }
            }

            IReadOnlyCollection<Task<OrderItem>> tasks = [.. request
                .Select(async p =>
                {
                    var product = await GetProductByIdAsync(p, ct);
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
            return true;
        }

        private async Task<Product?> GetProductByIdAsync(OrderItem p, CancellationToken ct)
        {
            return await unitOfWorks.Product.GetByIdAsync(p.Id, ct);
        }
    }
}

