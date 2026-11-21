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
            List<OrderItem> items = new(request.Count);
            foreach (var item in request)
            {
                var product = await unitOfWorks.Product.GetByIdAsync(item.ProductId, ct);
                if (product is null)
                {
                    return false;
                }

                item.Order = order;
                item.Price = product.Price;
                item.UnitQuantity = product.UnitQuantity;
                items.Add(item);
            }
            await unitOfWorks.OrderItems.AddRangeAsync(items, ct);
            return true;
        }

    }
}

