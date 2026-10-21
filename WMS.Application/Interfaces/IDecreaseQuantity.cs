
using WMS.Application.common.Service;

namespace WMS.Application.Interfaces
{
    public interface IDecreaseQuantity
    {
        Task<IReadOnlyList<StockResult>> DecreaseAsync<TItem>(
            IReadOnlyCollection<TItem> items,
            Func<TItem, string> barcodeSelector,
            Func<TItem, decimal> quantitySelector,
            CancellationToken ct);
    }
}
