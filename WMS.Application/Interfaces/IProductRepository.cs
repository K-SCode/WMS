using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken);
        Task<IDictionary<string, bool>> ExistsByBarcodes(IReadOnlyCollection<string> barcodes, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Product>> GetAllByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken);
    }
}
