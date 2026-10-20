using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Command.CancelOrder
{
    public sealed record CancelOrderCommand(int OrderId)
        : ICommand<Result<bool>>;
}
