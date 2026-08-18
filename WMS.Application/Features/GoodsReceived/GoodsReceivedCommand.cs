using Mediator;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived
{
    public sealed record ProductReceived(string Barcode, int Quantity);
    public sealed record GoodsReceivedCommand(
            IReadOnlyCollection<ProductReceived> Products) : ICommand<Result<Dictionary<GoodsReceivedStatus, IEnumerable<string>>>>;
}
