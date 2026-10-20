using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        public async Task AddAsync(Order order, CancellationToken ct)
        {
            await context.Set<Order>().AddAsync(order, ct);
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await context.Set<Order>().FirstOrDefaultAsync(o => o.Id == id, ct);
        }
    }
}