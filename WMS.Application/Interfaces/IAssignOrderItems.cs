using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IAssignOrderItems
    {
        Task Assing(
            IReadOnlyCollection<OrderItem> request,
            Order order,
            CancellationToken ct);
    }
}
