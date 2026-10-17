using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Features.GoodsReceived.Services;
using WMS.Core.Common.Enum;

namespace WMS.Application.Interfaces
{
    public interface ISaveGoodsReceipt
    {
        Task SaveInDatabase(
            Dictionary<ItemGrnStatus, IEnumerable<GoodsReceiptModel>> items,
            DocumentInformation information,
            CancellationToken ct);
    }
}
