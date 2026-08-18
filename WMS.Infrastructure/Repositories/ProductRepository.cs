using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : Repository<Product>(context), IProductRepository
    {
        public async Task<IDictionary<string,bool>> ExistsByBarcodes(IEnumerable<string> barcodes, CancellationToken cancellationToken)
        {
            var existingBarcodes = await _dbSet
                .Where(product => barcodes.Contains(product.Barcode))
                .Select(product => product.Barcode)
                .ToHashSetAsync(cancellationToken);

            return barcodes.Distinct().ToDictionary(
                barcode => barcode,
                existingBarcodes.Contains);
        }

        public async Task<Product?> GetByBarcodeAsync(string barcode,CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(product => product.Barcode == barcode, cancellationToken);
        }
    }
}
