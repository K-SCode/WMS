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
    }
}
