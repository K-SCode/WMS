using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer, CancellationToken ct);
        Task<Customer?> GetByEmailAsync(string email, CancellationToken ct);
    }
}
