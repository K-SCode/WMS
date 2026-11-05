using WMS.Application.common.Service;
using WMS.Core.Common.Enum;

namespace WMS.Application.Interfaces.Warehouse
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
