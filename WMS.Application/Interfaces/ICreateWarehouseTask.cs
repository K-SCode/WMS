using WMS.Application.common.Service;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface ICreateWarehouseTask
    {
        Task CreateTaskAsync(
            IReadOnlyCollection<StockResult> items,
            WarehouseSource source,
            int documentId,
            CancellationToken ct);
    }
}
