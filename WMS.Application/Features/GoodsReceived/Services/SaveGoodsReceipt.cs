using Mapster;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Services
{
    public class SaveGoodsReceipt(
        IUnitOfWorks unitOfWorks) : ISaveGoodsReceipt
    {
        public async Task SaveInDatabase(
            Dictionary<ItemGrnStatus, IEnumerable<GoodsReceiptModel>> items,
            DocumentInformation information,
            CancellationToken ct)
        {
            DateTime dateOfIssue = DateTime.Now;
            DateTime receivedAatUtc = DateTime.UtcNow;

            bool supplierExists = await unitOfWorks.Supplier.ExistsAsync(information.SupplierData.Nip, ct);
            if (!supplierExists)
            {
                await unitOfWorks.Supplier.AddAsync(information.SupplierData.Adapt<Supplier>(), ct);
            }

            GoodsReceipt goodsReceipt = new()
            {
                InvoiceNumber = information.InvoiceNumber,
                DateOfIssue = dateOfIssue,
                ReceivedAtUtc = receivedAatUtc,
                SupplierNip = information.SupplierData.Nip,
                Status = items.Keys.Any(tmp => tmp != ItemGrnStatus.Accepted) ? OrderStatus.InAnalysis : OrderStatus.Accepted,
                Comment = information.Comment
            };

            string grnNumber = $"GRN-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";
            goodsReceipt.GRNNumber = grnNumber;
            await unitOfWorks.GoodsReceipt.AddAsync(goodsReceipt, ct);
                

            List<GoodsReceiptsItems> productToAdd = items.Values
                .SelectMany(item => item)
                .Select(x =>
                {
                    var item = x.Adapt<GoodsReceiptsItems>();
                    item.GoodsReceipt = goodsReceipt;
                    return item;
                })
                .ToList();

            await unitOfWorks.GoodsReceiptsItems.AddRangeAsync(productToAdd, ct);
           
        }
    }
}

//Jak skoncze

//update bazy danych !!!!!!!!!!!!