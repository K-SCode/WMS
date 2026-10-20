using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IWarehouseRepository
    {
        Task AddRangeAsync(IEnumerable<WarehouseTask> tasks, CancellationToken ct);
    }
}
