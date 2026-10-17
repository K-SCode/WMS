using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class OrderItemsRepository(ApplicationDbContext context) : IOrderItemsRepository
    {
        public async Task AddRangeAsync(IEnumerable<OrderItem> items, CancellationToken ct)
        {
            await context.Set<OrderItem>().AddRangeAsync(items,ct);
        }
    }
}