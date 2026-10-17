using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IGoodsReceiptsItemsRepository
    {
        Task AddRangeAsync(IEnumerable<GoodsReceiptsItems> items, CancellationToken ct);
    }
}
