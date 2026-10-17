using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface ISupplierRepository
    {
        Task<bool> ExistsAsync(string nip, CancellationToken ct);
        Task AddAsync(Supplier supplier, CancellationToken ct);
    }
}
