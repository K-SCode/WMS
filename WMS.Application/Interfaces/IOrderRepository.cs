using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order,CancellationToken ct);
        Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    }
}
