using WMS.Application.common.Helper;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Warehouse;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.common.Service
{
    public class AssignWorker(
        IUnitOfWork unitOfWork,
        IWorkerDirectory workerDirectory) : IAssignWorker
    {
        public async Task AssignAsync(IReadOnlyCollection<WarehouseTask> tasks, CancellationToken ct)
        {
            var pending = tasks.Where(
                t => t.Status == WarehouseStatus.Pending).ToList();
            if (pending.Count == 0)
                return;

            var workers = await workerDirectory.GetActiveWarehousemenIdsAsync(ct);
            if (workers.Count == 0)
                return;

            var openTasks = await unitOfWork.Warehouse.CountOpenTasksByUserAsync(workers, ct);

            int chosen = workers.OrderBy(
                id => openTasks.TryGetValue(id, out int count) ? count : 0)
                .ThenBy(id => id)
                .First();

            foreach(var task in pending)
            {
                var result = AssignUserToTaskHelper.AssignTo(chosen, task);
                if (!result.IsSuccess)
                    return;
            }
        }
    }
}
