using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class StockRepository(ApplicationDbContext context) : Repository<Stock>(context), IStockRepository
    {
        public async Task<bool> StockExistsAsync(int productId, int locationId, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(stock => stock.ProductId == productId && stock.LocationId == locationId, ct);
        }

        public async Task<IDictionary<string, IReadOnlyCollection<Stock>>> GetNotFullStocksByProductBarcodesAsync(
            IReadOnlyCollection<string> barcodes,
            CancellationToken ct)
        {

            return await GetStocksGrouppedAsync(barcodes, stock => !stock.Localization.IsFull, ct);
        }

        public async Task<IDictionary<string, IReadOnlyCollection<Stock>>> GetStocksWithProductByBarcodeAsync(
            IReadOnlyCollection<string> barcodes,
            CancellationToken ct)
        {
            return await GetStocksGrouppedAsync(barcodes,
                stock => stock.Quantity > 0,
                ct);
        }

        private async Task<IDictionary<string, IReadOnlyCollection<Stock>>> GetStocksGrouppedAsync(
            IReadOnlyCollection<string> barcodes,
            Expression<Func<Stock,bool>> predicate,
            CancellationToken ct)
        {
            var distinctBarcodes = barcodes.Distinct().ToList();
            var stocks = await _dbSet
                .Include(stock => stock.Product)
                .Where(stock =>
                    distinctBarcodes.Contains(stock.Product.Barcode))
                .Where(predicate)
            .ToListAsync(ct);

            var grouped = stocks.GroupBy(stock => stock.Product.Barcode)
                .ToDictionary(group => group.Key, group => (IReadOnlyCollection<Stock>)group.ToList());

            return distinctBarcodes.ToDictionary(
                barcode => barcode,
                barcode => grouped.TryGetValue(barcode, out var list) ? list : Array.Empty<Stock>());
        }
    }
}
