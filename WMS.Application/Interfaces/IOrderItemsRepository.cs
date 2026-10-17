using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IOrderItemsRepository
    {
        Task AddRangeAsync(IEnumerable<OrderItem> items, CancellationToken ct);
    }
}
