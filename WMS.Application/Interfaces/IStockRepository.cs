using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<bool> StockExistsAsync(int productId, int locationId, CancellationToken ct);
        Task<Dictionary<string, IEnumerable<Stock>>> GetNotFullStocksByProductBarcodesAsync(IEnumerable<string> barcodes, CancellationToken ct);
    }
}
