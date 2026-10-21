using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Command.ShippedOrder
{
    public sealed record ShippedOrderCommand(int Id) : ICommand<Result<bool>>;
}
