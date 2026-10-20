using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Commands.GoodsReceived
{
    public sealed class GoodsReceivedCommandHandler(
        IGoodsReceivedProcess processor)
        : ICommandHandler<GoodsReceivedCommand, Result<bool>>
    {
        public async ValueTask<Result<bool>> Handle(
            GoodsReceivedCommand command, CancellationToken cancellationToken)
        {
            var result = await processor.ProcessAsync(command.GoodsReceivedNote, cancellationToken);

            return result;
        }

    }
}
