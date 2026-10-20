using WMS.Application.common.Service;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface ICreateWarehouseTask
    {
        Task CreateTaskForNotExistDocumentAsync<T>(
            IReadOnlyCollection<StockIncreaseResult> items,
            WarehouseSource source,
            Func<T, int> linkId,
            T document,
            CancellationToken ct);
        Task CreateTaskForExistDocumentAsync(
            IReadOnlyCollection<StockIncreaseResult> items,
            WarehouseSource source,
            int documentId,
            CancellationToken ct);
    }
}
