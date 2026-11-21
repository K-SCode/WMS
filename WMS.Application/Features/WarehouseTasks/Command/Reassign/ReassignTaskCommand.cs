using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.WarehouseTasks.Command.Reassign
{
    public sealed record ReassignTaskCommand(int Id, int UserId) : ICommand<Result<bool>>;
}
