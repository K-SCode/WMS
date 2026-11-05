using WMS.Core.Common.Enum;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces.Orders
{
    public interface IOrderRestocking
    {
        Task<bool> Restock(Order order, WarehouseSource source, CancellationToken ct);
    }
}
