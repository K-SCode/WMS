using WMS.Application.common.Service;

namespace WMS.Application.Interfaces.Stocks
{
    public interface IDecreaseQuantity
    {
        Task<IReadOnlyCollection<StockResult>> DecreaseAsync<TItem>(
            IReadOnlyCollection<TItem> items,
            Func<TItem, string> barcodeSelector,
            Func<TItem, decimal> quantitySelector,
            CancellationToken ct);
    }
}
