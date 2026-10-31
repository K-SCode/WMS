using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.GoodsReceived.Query.Get
{
    public sealed record GetGoodsReceivedQuery(int Id) : IQuery<Result<GoodsReceivedDetailsDto>>;
}
