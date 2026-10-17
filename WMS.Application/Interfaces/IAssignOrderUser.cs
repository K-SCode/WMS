using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IAssignOrderUser
    {
        Task<Customer> AssignAsync(Customer request, CancellationToken ct);
    }
}
