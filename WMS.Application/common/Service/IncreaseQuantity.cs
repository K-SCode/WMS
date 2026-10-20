using WMS.Application.Interfaces;
using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.common.Service
{
    public class IncreaseQunatity(IUnitOfWorks unitOfWorks) : IIncreaseQuantity
    {
        public async Task<IReadOnlyList<StockIncreaseResult>> IncreaseAsync<TItem>(
            IReadOnlyCollection<TItem> items,
            Func<TItem, string> barcodeSelector,
            Func<TItem, decimal> quantitySelector,
            CancellationToken ct)
        {

            List<string> barcodes = [.. items.Select(barcodeSelector).Distinct()];

            IDictionary<string, bool> exists =
                await GetExistsProductsAsync(barcodes, ct);

            IDictionary<string, IEnumerable<Stock>> notFullStocks =
                await GetNotFullStocksAsync(barcodes, ct);

            var results = new List<StockIncreaseResult>(items.Count);


            foreach (var item in items)
            {
                decimal quantity = quantitySelector(item);
                string barcode = barcodeSelector(item);
                IReadOnlyCollection<ResolveStatus> resolveResults =
                    Resolve(
                        barcode,
                        quantity,
                        exists,
                        notFullStocks
                    );

                results.Add(new(barcode, resolveResults));
            }

            return results;
        }
        private static List<ResolveStatus> Resolve(
            string barcode,
            decimal quantity,
            IDictionary<string, bool> existing,
            IDictionary<string, IEnumerable<Stock>> notFullStocks
            )
        {
            existing.TryGetValue(barcode, out bool exists);
            if (!exists)
                return [new ResolveStatus(
                    null,
                    StockIncreaseStatus.ProductNotFound,
                    quantity)];

            if (!notFullStocks.TryGetValue(barcode, out var stocks))
                return [new ResolveStatus(
                    null,
                    StockIncreaseStatus.OutOfSapce,
                    quantity)];

            var distribution =
                DistributeQuantity(stocks, quantity);

            decimal assignedTotal = distribution.Sum(d => d.Assigned);
            decimal remaining = quantity - assignedTotal;

            var rowStatus = remaining == 0
                ? StockIncreaseStatus.Accepted
                : StockIncreaseStatus.PartiallyAccepted;
            var results = distribution.Select(
                d => new ResolveStatus(d.Stock.LocationId, rowStatus, d.Assigned)).ToList();

            if (remaining > 0)
                results.Add(new ResolveStatus(null, StockIncreaseStatus.OutOfSapce, remaining));

           return results;
        }
        public sealed record ResolveStatus(int? LocationId, StockIncreaseStatus Status, decimal AssignedQuantity);

        private static List<(Stock Stock, decimal Assigned)> DistributeQuantity(IEnumerable<Stock> stocks, decimal quantity)
        {
            var assigned = new List<(Stock, decimal)>();

            foreach (Stock stock in stocks)
            {
                if (quantity == 0)
                    break;

                decimal freeSpace = Math.Max(0, stock.MaxQuantity - stock.Quantity);
                if (freeSpace == 0)
                    continue;
                decimal toAdd = Math.Min(quantity, freeSpace);
                stock.Quantity += toAdd;
                quantity -= toAdd;

                assigned.Add((stock, toAdd));
            }
            return assigned;
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
    }
}
