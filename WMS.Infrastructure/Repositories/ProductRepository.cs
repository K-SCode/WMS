using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : Repository<Product>(context), IProductRepository
    {
        public async Task<Product?> GetByBarcodeAsync(string barcode,CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(product => product.Barcode == barcode, cancellationToken);
        }
    }
}
