using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.common.Service
{
    public class CreateWarehouseTask(IUnitOfWorks unitOfWorks) : ICreateWarehouseTask
    {
        public async Task CreateTaskAsync(
            IReadOnlyCollection<StockIncreaseResult> items,
            WarehouseSource source,
            int documentId,
            CancellationToken ct)
        {
            List<WarehouseTask> task = new(items.Count);

            foreach (StockIncreaseResult item in items)
            {
                foreach (var stock in item.ResolveStatuses)
                {
                    task.Add(new(
                        item.Barcode,
                        stock.AssignedQuantity,
                        documentId,
                        source,
                        stock.Status,
                        stock.LocationId));

                }
            }

            await unitOfWorks.Warehouse.AddRangeAsync(task, ct);
        }
    }
}
