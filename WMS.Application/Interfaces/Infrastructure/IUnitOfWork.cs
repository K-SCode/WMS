using Microsoft.EntityFrameworkCore.Storage;
using WMS.Application.Interfaces.Repositories;

namespace WMS.Application.Interfaces.Infrastructure
{
    public interface IUnitOfWork
    {
        ILocationRepository Location { get; }
        IProductRepository Product { get; }
        IStockRepository Stock { get; }
        IGoodsReceiptRepository GoodsReceipt { get; }
        ISupplierRepository Supplier { get; }
        IGoodsReceiptsItemsRepository GoodsReceiptsItems{ get; }
        IOrderRepository Order { get; }
        ICustomerRepository Customer { get; }
        IOrderItemsRepository OrderItems { get; }
        IWarehouseRepository Warehouse { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
