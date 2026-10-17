using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class SupplierRespository(ApplicationDbContext context) : ISupplierRepository
    {
        public async Task AddAsync(Supplier supplier, CancellationToken ct)
        {
            await context.Set<Supplier>().AddAsync(supplier,ct);
        }

        public async Task<bool> ExistsAsync(string nip, CancellationToken ct)
        {
            return await context.Set<Supplier>().AnyAsync(tmp => tmp.Nip == nip, ct);
        }
    }
}
