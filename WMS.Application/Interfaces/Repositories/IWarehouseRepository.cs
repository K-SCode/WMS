using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Repositories
{
    public interface IWarehouseRepository
    {
        Task AddRangeAsync(IEnumerable<WarehouseTask> tasks, CancellationToken ct);
        Task<WarehouseTask?> GetByIdAsync(int id, CancellationToken ct);
        Task<IDictionary<int, int>> CountOpenTasksByUserAsync(
            IReadOnlyCollection<int> userIds,
            CancellationToken ct);
    }
}
