using WMS.Application.Interfaces.Repositories;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WMS.Infrastructure.Repositories
{
    public class WarehouseRepository(ApplicationDbContext context) : IWarehouseRepository
    {
        public async Task AddRangeAsync(IEnumerable<WarehouseTask> tasks, CancellationToken ct)
        {
            await context.Set<WarehouseTask>().AddRangeAsync(tasks, ct);
        }

        public async Task<IDictionary<int, int>> CountOpenTasksByUserAsync(IReadOnlyCollection<int> userIds, CancellationToken ct)
        {
            return await context.Set<WarehouseTask>()
                .Where(t => t.AssignedUserId != null
                    && userIds.Contains(t.AssignedUserId.Value)
                    && (t.Status == WarehouseStatus.Assigned || t.Status == WarehouseStatus.InProgress))
                .GroupBy(t => t.AssignedUserId!.Value)
                .Select(group => new { UserId = group.Key, Count = group.Count() })
                .ToDictionaryAsync(x => x.UserId, x => x.Count, ct);
        }

        public async Task<WarehouseTask?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await context.Set<WarehouseTask>().FindAsync(id, ct);
        }
    }
}
