using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.GoodsReceived.Commands.ImportFileGoodsReceived
{
    public sealed record ImportFileGoodsReceivedCommand(
        Stream FileStream,
        string Format,
        DocumentInformation Information) :
         ICommand<Result<bool>>;
}
