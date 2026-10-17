using Mapster;
using WMS.Application.Features.GoodsReceived.Commands.GoodsReceived;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Services
{
    public class GoodsReceivedProcess(
        IIncreaseStock increaseStock,
        ISaveGoodsReceipt grn,
        IUnitOfWorks unitOfWorks) : IGoodsReceivedProcess
    {
        public async Task<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceivedDto>>>ProcessAsync(
            GoodsReceivedNote goodsReceived,
            CancellationToken ct)
        {
            Dictionary<ItemGrnStatus, IEnumerable<GoodsReceiptModel>> goodsReceipt = await increaseStock.ProcessAsync(goodsReceived.Products, ct);

            await grn.SaveInDatabase(goodsReceipt, goodsReceived.Information, ct);

            var result = goodsReceipt.Adapt<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceivedDto>>>();

            await unitOfWorks.SaveChangesAsync(ct);
            return result;
        }
    }

    public record GoodsReceiptModel(string Barcode, ItemGrnStatus Status, decimal Quantity, string UnitQuantity, decimal PurchasePrice);
}
