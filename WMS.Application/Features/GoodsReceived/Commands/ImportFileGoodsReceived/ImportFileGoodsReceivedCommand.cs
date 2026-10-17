using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived
{
    public sealed record ImportFileGoodsReceivedCommand(Stream FileStream, string Format, DocumentInformation Information) :
         ICommand<Result<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceivedDto>>>>;
}
