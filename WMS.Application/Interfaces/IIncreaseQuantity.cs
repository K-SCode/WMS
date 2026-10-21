using WMS.Application.common.Service;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IIncreaseQuantity
    {
        Task<IReadOnlyList<StockResult>> IncreaseAsync<TItem>(
            IReadOnlyCollection<TItem> items,
            Func<TItem, string> barcodeSelector,
            Func<TItem, decimal> quantitySelector,
            CancellationToken ct);
    }
}
