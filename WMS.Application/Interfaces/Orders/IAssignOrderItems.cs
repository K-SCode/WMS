using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Orders
{
    public interface IAssignOrderItems
    {
        Task<bool> Assing(
            IReadOnlyCollection<OrderItem> request,
            Order order,
            CancellationToken ct);
    }
}
