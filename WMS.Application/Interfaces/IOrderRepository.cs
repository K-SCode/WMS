using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order,CancellationToken ct);
    }
}
