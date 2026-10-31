using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.GoodsReceived.Query.List
{
    public sealed record ListGoodsReceivedQuery : IQuery<Result<IReadOnlyCollection<GoodsReceivedDto>>>;
}
