using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Orders
{
    public interface IAssignOrderUser
    {
        Task<Customer> AssignAsync(Customer request, CancellationToken ct);
    }
}
