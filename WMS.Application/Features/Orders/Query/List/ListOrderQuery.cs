using Mediator;
using WMS.Application.Features.Orders.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Query.List
{
    public record ListOrderQuery : IQuery<Result<IReadOnlyCollection<OrderDto>>>;
}
