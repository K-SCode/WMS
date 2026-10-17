using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Services
{
    public class IncreaseStock(IUnitOfWorks unitOfWorks) : IIncreaseStock
    {
        public async Task<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceiptModel>>> ProcessAsync(
            IEnumerable<ProductReceived> products,
            CancellationToken ct)
        {
            var resultList = new List<GoodsReceiptModel>();
            var existsProducts = await GetExistsProductsAsync(
                products.Select(p => p.Barcode), ct);
            var notFullStocks = await GetNotFullStocksAsync(
                products.Select(p => p.Barcode), ct);

            foreach (var product in products)
            {
                existsProducts.TryGetValue(product.Barcode, out bool exists);
                if (!exists)
                {
                    AddItemToList(
                        resultList,
                        product.Barcode,
                        ItemGrnStatus.ProductNotFound,
                        product.Quantity,
                        product.UnitQuantity,
                        product.PurchasePrice);
                    continue;
                }

                if (notFullStocks.TryGetValue(
                    product.Barcode, out IEnumerable<Stock>? stocks) && stocks.Any())
                {
                    var remainingQuantity = await TryIncreaseQuantityAsync(
                        stocks, product.Quantity);

                    var status = remainingQuantity == 0 ? ItemGrnStatus.Accepted : ItemGrnStatus.OutOfSapce;
                    AddItemToList(resultList,
                        product.Barcode,
                        status,
                        product.Quantity,
                        product.UnitQuantity,
                        product.PurchasePrice);
                }
                else
                {
                    AddItemToList(
                        resultList,
                        product.Barcode,
                        ItemGrnStatus.OutOfSapce,
                        product.Quantity,
                        product.UnitQuantity,
                        product.PurchasePrice);
                }
            }

            var resultGouped = resultList.GroupBy(
                goodsReceived => goodsReceived.Status)
                .ToDictionary(
                group => group.Key,
                group => group.AsEnumerable()
                );
            return resultGouped;
        }

        private async Task<IDictionary<string, bool>> GetExistsProductsAsync(
            IEnumerable<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Product.ExistsByBarcodes(barcodes, ct);
        }
        private async Task<IDictionary<string, IEnumerable<Stock>>> GetNotFullStocksAsync(
            IEnumerable<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Stock.GetNotFullStocksByProductBarcodesAsync(
                barcodes, ct);
        }

        private async Task<decimal> TryIncreaseQuantityAsync(
            IEnumerable<Stock> stocks, decimal quantity)
        {
            foreach (var stock in stocks)
            {
                quantity = await IncreaseAsync(stock, quantity);
                if (quantity == 0)
                    break;
            }
            return quantity;
        }
        private static async Task<decimal> IncreaseAsync(
            Stock stock, decimal quantity)
        {
            if (stock.Quantity + quantity <= stock.MaxQuantity)
            {
                stock.Quantity += quantity;
                return 0;
            }
            decimal freeSpace = stock.MaxQuantity - stock.Quantity;
            stock.Quantity += freeSpace;
            return quantity -= freeSpace;
        }
        private static void AddItemToList(
            List<GoodsReceiptModel> list,
            string barcode,
            ItemGrnStatus status,
            decimal quantity,
            string unit,
            decimal price)
        {
            list.Add(new(barcode, status, quantity, unit, price));
        }
    }
}
