using Microsoft.EntityFrameworkCore;
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

        public async Task<Dictionary<string, IEnumerable<Stock>>> GetNotFullStocksByProductBarcodesAsync(IEnumerable<string> barcodes, CancellationToken ct)
        {
          
            var freeStocks = await _dbSet.Include(stock => stock.Product)
                .Where(stock => barcodes.Contains(stock.Product.Barcode) && !stock.Localization.IsFull)
            .ToListAsync(ct);

            var stocks = freeStocks.GroupBy(stock => stock.Product.Barcode)
                .ToDictionary(group => group.Key, group => group.AsEnumerable());

            return barcodes.ToDictionary(
                barcode => barcode,
                barcode => stocks.TryGetValue(barcode, out var list) ? list : Enumerable.Empty<Stock>());
        }
    }
}
