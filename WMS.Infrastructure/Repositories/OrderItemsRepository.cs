using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class OrderItemsRepository(ApplicationDbContext context) : IOrderItemsRepository
    {
        public async Task AddRangeAsync(IReadOnlyCollection<OrderItem> items, CancellationToken ct)
        {
            await context.Set<OrderItem>().AddRangeAsync(items,ct);
        }
        
        public async Task<IReadOnlyCollection<OrderItem>> GetAllByOrderIdAsync(int OrderId, CancellationToken ct)
        {
            return await context.Set<OrderItem>()
                .Include(i => i.Product)
                .Where(o => o.OrderId == OrderId)
                .ToListAsync(ct);
        }
    }
}