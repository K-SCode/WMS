using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IProductRepository : IStockRepository<Product>
    {
        Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken);
        Task<IDictionary<string, bool>> ExistsByBarcodes(IEnumerable<string> barcodes, CancellationToken cancellationToken);
    }
}
