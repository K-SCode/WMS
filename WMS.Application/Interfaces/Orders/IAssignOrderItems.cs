using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Orders
{
    public interface IAssignOrderItems
    {
        Task Assing(
            IReadOnlyCollection<OrderItem> request,
            Order order,
            CancellationToken ct);
    }
}
