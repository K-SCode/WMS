using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IOrderRegistration
    {
        Task<Order> RegisterAsync(Order request, Customer customer, CancellationToken ct);
    }
}
