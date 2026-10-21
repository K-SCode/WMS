using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.common.Service
{
    public partial class DecreaseQuantity(IUnitOfWorks unitOfWorks) : IDecreaseQuantity
    {
        public async Task<IReadOnlyList<StockResult>> DecreaseAsync<TItem>(
         IReadOnlyCollection<TItem> items,
         Func<TItem, string> barcodeSelector,
         Func<TItem, decimal> quantitySelector,
         CancellationToken ct)
        {

            List<string> barcodes = [.. items.Select(barcodeSelector).Distinct()];

            IDictionary<string, bool> exists =
                await GetExistsProductsAsync(barcodes, ct);

            IDictionary<string, IReadOnlyCollection<Stock>> stocksWithProduct =
                await GetStocksWithProductAsync(barcodes, ct);

            var results = new List<StockResult>(items.Count);


            foreach (var item in items)
            {
                decimal quantity = quantitySelector(item);
                string barcode = barcodeSelector(item);
                IReadOnlyCollection<ResolveStatus> resolveResults =
                    Resolve(
                        barcode,
                        quantity,
                        exists,
                        stocksWithProduct
                    );

                results.Add(new(barcode, resolveResults));
            }

            return results;
        }
        private static List<ResolveStatus> Resolve(
            string barcode,
            decimal quantity,
            IDictionary<string, bool> existing,
            IDictionary<string, IReadOnlyCollection<Stock>> stocksWithProduct
            )
        {
            existing.TryGetValue(barcode, out bool exists);
            if (!exists)
                return [new ResolveStatus(
                    null,
                    StockIncreaseStatus.ProductNotFound,
                    quantity)];

            if (!stocksWithProduct.TryGetValue(barcode, out var stocks) || stocks.Count ==  0)
                return [new ResolveStatus(
                    null,
                    StockIncreaseStatus.StocksEmpty,
                    quantity)];

            var distribution =
                DistributeQuantity(stocks, quantity);

            decimal assignedTotal = distribution.Sum(d => d.TakeQuantity);
            decimal remaining = quantity - assignedTotal;

            var rowStatus = remaining == 0
                ? StockIncreaseStatus.Accepted
                : StockIncreaseStatus.ProductNotFound;
            var results = distribution.Select(
                d => new ResolveStatus(d.Stock.LocationId, rowStatus, d.TakeQuantity)).ToList();

            if (remaining > 0)
                results.Add(new ResolveStatus(null, StockIncreaseStatus.ProductNotFound, remaining));

            return results;
        }

        private static List<(Stock Stock, decimal TakeQuantity)> DistributeQuantity(IReadOnlyCollection<Stock> stocks, decimal quantity)
        {
            var decrease = new List<(Stock, decimal)>();

            foreach (Stock stock in stocks)
            {
                if (quantity == 0)
                    break;

                if (stock.Quantity <= 0)
                {
                    continue;
                }

                decimal takeQuantity = Math.Min(stock.Quantity, quantity);

                if (takeQuantity <=0)
                    continue;
                stock.Quantity -= takeQuantity;
                quantity -= takeQuantity;

                decrease.Add((stock, takeQuantity));
            }
            return decrease;
        }

        private async Task<IDictionary<string, bool>> GetExistsProductsAsync(
            IReadOnlyCollection<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Product.ExistsByBarcodes(barcodes, ct);
        }
        private async Task<IDictionary<string, IReadOnlyCollection<Stock>>>  GetStocksWithProductAsync(
            IReadOnlyCollection<string> barcodes, CancellationToken ct)
        {
            return await unitOfWorks.Stock.GetStocksWithProductByBarcodeAsync(
                barcodes, ct);
        }
    }
}
