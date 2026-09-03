using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Commands.GoodsReceived
{
    public sealed class GoodsReceivedCommandHandler(IGoodsReceivedProcessor processor) : ICommandHandler<GoodsReceivedCommand, Result<Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>>>>
    {
        public async ValueTask<Result<Dictionary<GoodsReceivedStatus,IEnumerable<GoodsReceivedDto>>>> Handle(GoodsReceivedCommand command, CancellationToken cancellationToken)
        {

            var products = command.Products;

            var result = await processor.ProcessAsync(products, cancellationToken);

            return result;
        }
       
    }
}
