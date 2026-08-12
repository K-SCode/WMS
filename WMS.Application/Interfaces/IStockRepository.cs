using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IStockRepository : IStockRepository<Stock>
    {
        Task<bool> StockExistsAsync(int productId, int locationId, CancellationToken ct);
    }
}
