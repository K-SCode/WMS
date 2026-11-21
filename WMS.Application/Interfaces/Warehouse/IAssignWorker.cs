using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Warehouse
{
    public interface IAssignWorker
    {
        Task AssignAsync(IReadOnlyCollection<WarehouseTask> tasks, CancellationToken ct);
    }
}
