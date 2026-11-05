using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces.GoodsReceipts
{
    public interface ISaveGoodsReceipt
    {
        Task<GoodsReceipt> SaveInDatabaseAsync(
            IEnumerable<ProductReceived> items,
            DocumentInformation information,
            CancellationToken ct);
    }
}
