using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Warehouse;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.common.Service
{
    public class CreateWarehouseTask(
        IUnitOfWork unitOfWorks,
        IAssignWorker assignWorker) : ICreateWarehouseTask
    {
        public async Task CreateTaskAsync(
            IReadOnlyCollection<StockResult> items,
            WarehouseSource source,
            int documentId,
            CancellationToken ct)
        {
            List<WarehouseTask> task = new(items.Count);

            foreach (StockResult item in items)
            {
                foreach (var stock in item.ResolveStatuses)
                {
                    task.Add(new(
                        item.Barcode,
                        stock.Quantity,
                        documentId,
                        source,
                        stock.Status,
                        stock.LocationId));

                }
            }

            await assignWorker.AssignAsync(task, ct);

            await unitOfWorks.Warehouse.AddRangeAsync(task, ct);
        }
    }
}
