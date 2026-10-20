using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Services
{
    public class GoodsReceivedProcess(
        IIncreaseQuantity increaseQuantity,
        ISaveGoodsReceipt saveGoodsReceipt,
        ICreateWarehouseTask createWarehouseTask,
        IUnitOfWorks unitOfWorks) : IGoodsReceivedProcess
    {
        public async Task<bool>ProcessAsync(
            GoodsReceivedNote goodsReceived,
            CancellationToken ct)
        {
            await using var transaction = await unitOfWorks.BeginTransactionAsync(ct);
            
            GoodsReceipt grn = await saveGoodsReceipt.SaveInDatabaseAsync(goodsReceived.Products, goodsReceived.Information, ct);

            await unitOfWorks.SaveChangesAsync(ct);

            var increaseResult =
                await increaseQuantity.IncreaseAsync(
                    goodsReceived.Products,
                    p => p.Barcode,
                    p => p.Quantity,
                    ct);

            await createWarehouseTask.CreateTaskAsync(increaseResult, WarehouseSource.GoodsReceiptOverflow,  grn.Id,ct);

            await unitOfWorks.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
            return true;
        }


    }
}
