using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.GoodsReceived.Commands.GoodsReceived
{
    public sealed record GoodsReceivedCommand(
            GoodsReceivedNote GoodsReceivedNote) : ICommand<Result<bool>>;
}
