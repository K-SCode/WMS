using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived.Services
{
    public class GoodsReceivedProcessor(IUnitOfWorks unitOfWorks) : IGoodsReceivedProcessor
    {
        public async Task<Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>>> ProcessAsync(
            IEnumerable<ProductReceived> products,
            CancellationToken ct)
        {
            var resultList = new List<GoodsReceivedDto>();
            var existsProducts = await GetExistsProductsAsync(products.Select(p => p.Barcode), ct);
            var notFullStocks = await GetNotFullStocksAsync(products.Select(p => p.Barcode), ct);

            foreach (var product in products)
            {
                existsProducts.TryGetValue(product.Barcode, out bool exists);
                if (!exists)
                {
                    resultList.Add(new(product.Barcode, GoodsReceivedStatus.ProductNotFound, product.Quantity));
                    continue;
                }

                if (notFullStocks.TryGetValue(product.Barcode, out IEnumerable<Stock>? stocks) && stocks.Any())
                {
                    var remainingQuantity = await TryIncreaseQuantityAsync(stocks, product.Quantity);

                    var status = remainingQuantity == 0 ? GoodsReceivedStatus.Success : GoodsReceivedStatus.OutOfSpace;
                    resultList.Add(new(product.Barcode, status, remainingQuantity));
                }
                else
                {
                    resultList.Add(new(product.Barcode, GoodsReceivedStatus.OutOfSpace, product.Quantity));
                }
            }

            var resultGouped = resultList.GroupBy(goodsReceived => goodsReceived.Status)
                .ToDictionary(
                group => group.Key,
                group => group.AsEnumerable()
                );
            await unitOfWorks.SaveChangesAsync(ct );
            return resultGouped;
        }

        private async Task<IDictionary<string, bool>> GetExistsProductsAsync(IEnumerable<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Product.ExistsByBarcodes(barcodes, ct);
        }
        private async Task<IDictionary<string, IEnumerable<Stock>>> GetNotFullStocksAsync(IEnumerable<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Stock.GetNotFullStocksByProductBarcodesAsync(barcodes, ct);
        }

        private async Task<int> TryIncreaseQuantityAsync(IEnumerable<Stock> stocks, int quantity)
        {
            foreach (var stock in stocks)
            {
                quantity = await IncreaseAsync(stock, quantity);
                if (quantity == 0)
                    break;
            }
            return quantity;
        }
        private async Task<int> IncreaseAsync(Stock stock, int quantity)
        {
            if (stock.Quantity + quantity <= stock.MaxQuantity)
            {
                stock.Quantity += quantity;
                return 0;
            }
            int freeSpace = stock.MaxQuantity - stock.Quantity;
            stock.Quantity += freeSpace;
            return quantity -= freeSpace;
        }
    }
}
