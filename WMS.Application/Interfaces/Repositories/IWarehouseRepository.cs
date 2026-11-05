using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Repositories
{
    public interface IWarehouseRepository
    {
        Task AddRangeAsync(IEnumerable<WarehouseTask> tasks, CancellationToken ct);
    }
}
