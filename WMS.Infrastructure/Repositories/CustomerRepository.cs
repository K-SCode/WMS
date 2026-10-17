using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
    {
        public async Task AddAsync(Customer customer,CancellationToken ct)
        {
            await context.Set<Customer>().AddAsync(customer, ct);
        }

        public async Task<Customer?> GetByEmailAsync(string email,CancellationToken ct)
        {
            return await context.Set<Customer>().FirstOrDefaultAsync(e => e.Email == email,ct);
        }
    }
}