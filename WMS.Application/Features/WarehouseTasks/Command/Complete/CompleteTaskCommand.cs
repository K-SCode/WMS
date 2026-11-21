using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.WarehouseTasks.Command.Complete
{
    public sealed record CompleteTaskCommand(int Id) : ICommand<Result<bool>>;
}
