using Mediator;
using WMS.Application.Features.WarehouseTasks.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.WarehouseTasks.Query
{
    public sealed record GetMyTasksQuery : IQuery<Result<IReadOnlyCollection<WarehouseTaskDto>>>;
}
