using WMS.Application.Features.GoodsReceived.Dto;

namespace WMS.Application.Interfaces.GoodsReceipts
{
    public interface IGoodsReceivedProcess
    {
        Task<bool> ProcessAsync(
            GoodsReceivedNote goodsReceived,
            CancellationToken ct);
    }
}
