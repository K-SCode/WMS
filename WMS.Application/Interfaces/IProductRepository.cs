using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken);
    }
}
