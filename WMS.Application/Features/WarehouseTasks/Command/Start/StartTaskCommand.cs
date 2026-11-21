using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.WarehouseTasks.Command.Start
{
    public sealed record StartTaskCommand(int Id) : ICommand<Result<bool>>;
}
