using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Commands.GoodsReceived
{
    public sealed record GoodsReceivedCommand(
            IReadOnlyCollection<ProductReceived> Products) : ICommand<Result<Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>>>>;
}
