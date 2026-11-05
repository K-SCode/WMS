using WMS.Application.common.Service;

namespace WMS.Application.Interfaces.Stocks
{
    public interface IIncreaseQuantity
    {
        Task<IReadOnlyList<StockResult?>> IncreaseAsync<TItem>(
            IReadOnlyCollection<TItem> items,
            Func<TItem, string> barcodeSelector,
            Func<TItem, decimal> quantitySelector,
            CancellationToken ct);
    }
}
