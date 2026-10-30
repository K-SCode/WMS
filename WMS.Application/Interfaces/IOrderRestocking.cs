using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IOrderRestocking
    {
        Task Restock(Order order, WarehouseSource source, CancellationToken ct);
    }
}
