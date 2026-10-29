using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Command.ReturnOrder
{
    public sealed record ReturnOrderCommand(int Id) : ICommand<Result<bool>>;
}
