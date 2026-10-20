using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IOrderItemsRepository
    {
        Task AddRangeAsync(IReadOnlyCollection<OrderItem> items, CancellationToken ct);
        Task<IReadOnlyCollection<OrderItem>> GetAllByOrderIdAsync(int OrderId, CancellationToken ct);
    }
}
