using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common.Enum;

namespace WMS.Application.Interfaces
{
    public interface IGoodsReceivedProcess
    {
        Task<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceivedDto>>> ProcessAsync(
            GoodsReceivedNote goodsReceived,
            CancellationToken ct);
    }
}
