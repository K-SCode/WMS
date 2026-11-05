using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order,CancellationToken ct);
        Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    }
}
