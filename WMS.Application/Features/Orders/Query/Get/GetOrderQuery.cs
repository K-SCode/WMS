using Mediator;
using WMS.Application.Features.Orders.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.Orders.Query.Get
{
    public record GetOrderQuery(int Id) : IQuery<Result<OrderDetailsDto>>;
}
