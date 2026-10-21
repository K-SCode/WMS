using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<bool> StockExistsAsync(int productId, int locationId, CancellationToken ct);
        Task<IDictionary<string, IReadOnlyCollection<Stock>>> GetNotFullStocksByProductBarcodesAsync(IReadOnlyCollection<string> barcodes, CancellationToken ct);
        Task<IDictionary<string, IReadOnlyCollection<Stock>>> GetStocksWithProductByBarcodeAsync(IReadOnlyCollection<string> barcodes, CancellationToken ct);
    }
}
