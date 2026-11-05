using Mapster;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces.GoodsReceipts;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Services
{
    public class SaveGoodsReceipt(
        IUnitOfWork unitOfWorks) : ISaveGoodsReceipt
    {
        public async Task<GoodsReceipt> SaveInDatabaseAsync(
            IEnumerable<ProductReceived> products,
            DocumentInformation information,
            CancellationToken ct)
        {
            DateTimeOffset dateOfIssue = DateTimeOffset.Now;
            DateTimeOffset receivedAatUtc = DateTimeOffset.UtcNow;

            await EnsureSupplierExiststAsync(information, ct);

            GoodsReceipt goodsReceipt = new()
            {
                InvoiceNumber = information.InvoiceNumber,
                DateOfIssue = dateOfIssue,
                ReceivedAtUtc = receivedAatUtc,
                SupplierNip = information.SupplierData.Nip,
                Comment = information.Comment,
                GRNNumber = GetGrnNumber()
            };

            await unitOfWorks.GoodsReceipt.AddAsync(goodsReceipt, ct);
            await AddItemsToReceipt(products, goodsReceipt, ct);

            return goodsReceipt;
        }

        private async Task AddItemsToReceipt(
            IEnumerable<ProductReceived> products,
            GoodsReceipt goodsReceipt,
            CancellationToken ct)
        {
            var items = products.Adapt<List<GoodsReceiptsItems>>();
            foreach (GoodsReceiptsItems item in items)
                item.GoodsReceipt = goodsReceipt;

            await unitOfWorks.GoodsReceiptsItems.AddRangeAsync(items, ct);
        }

        private static string GetGrnNumber()
        {
            return $"GRN-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";
        }

        private async Task EnsureSupplierExiststAsync(DocumentInformation information, CancellationToken ct)
        {
            bool supplierExists = await unitOfWorks.Supplier.ExistsAsync(information.SupplierData.Nip, ct);
            if (!supplierExists)
            {
                await unitOfWorks.Supplier.AddAsync(information.SupplierData.Adapt<Supplier>(), ct);
            }
        }
    }
}