using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Orders
{
    public interface IAssignCustomerOrder
    {
        Task<Customer> AssignAsync(Customer request, CancellationToken ct);
    }
}
