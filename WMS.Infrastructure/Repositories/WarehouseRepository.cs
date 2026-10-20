using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class WarehouseRepository(ApplicationDbContext context) : IWarehouseRepository
    {
        public async Task AddRangeAsync(IEnumerable<WarehouseTask> tasks, CancellationToken ct)
        {
            await context.Set<WarehouseTask>().AddRangeAsync(tasks, ct);
        }
    }
}
