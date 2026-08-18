using Mediator;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Features.GoodsReceived
{
    public sealed class GoodsReceivedCommandHandler(IUnitOfWorks unitOfWorks) : ICommandHandler<GoodsReceivedCommand, Result<Dictionary<GoodsReceivedStatus, IEnumerable<string>>>>
    {
        public async ValueTask<Result<Dictionary<GoodsReceivedStatus,IEnumerable<string>>>> Handle(GoodsReceivedCommand command, CancellationToken cancellationToken)
        {

            var products = command.Products;
            var resultList = new List<GoodsReceivedDto>();
            var existsProducts = await GetExistsProductsAsync(products.Select(p => p.Barcode), cancellationToken);
            var notFullStocks = await GetNotFullStocksAsync(products.Select(p => p.Barcode), cancellationToken);

            foreach(var product in products)
            {
                existsProducts.TryGetValue(product.Barcode, out bool exists);
                if (!exists)
                {
                    resultList.Add(new(product.Barcode, GoodsReceivedStatus.ProductNotFound, product.Quantity));
                    continue;
                }

                notFullStocks.TryGetValue(product.Barcode, out IEnumerable<Stock> stocks);
                if (stocks.Any())
                {
                    var remainingQuantity = await TryIncreaseQuantityAsync(stocks, product.Quantity, cancellationToken);
                    
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
                group => group.Select(goodsReceived => goodsReceived.Barcode).AsEnumerable()
                );
            

            await unitOfWorks.SaveChangesAsync(cancellationToken);
            return resultGouped;
        }
        private async Task<IDictionary<string,bool>> GetExistsProductsAsync(IEnumerable<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Product.ExistsByBarcodes(barcodes, ct);
        }
        private async Task<IDictionary<string,IEnumerable<Stock>>> GetNotFullStocksAsync(IEnumerable<string> barcodes,CancellationToken ct)
        {
            return await unitOfWorks.Stock.GetNotFullStocksByProductBarcodesAsync(barcodes, ct);
        }
     
        private async Task<int> TryIncreaseQuantityAsync(IEnumerable<Stock> stocks, int quantity,CancellationToken ct)
        {
            foreach(var stock in stocks)
            {
                quantity = await IncreaseAsync(stock,quantity, ct);
                if (quantity == 0)
                    break;
            }
            return quantity;
        }
        private async Task<int> IncreaseAsync(Stock stock, int quantity,  CancellationToken ct)
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
