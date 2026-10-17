using Mediator;
using WMS.Application.Features.Orders.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Command.CreateOrder
{
    public sealed record CreateOrderCommand(
        OrderInformation Information, CustomerInformation Customer, IReadOnlyList<CreateProductOrder> Products)
        : ICommand<Result<int>>;
}
