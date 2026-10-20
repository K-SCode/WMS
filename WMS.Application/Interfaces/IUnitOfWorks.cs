using Microsoft.EntityFrameworkCore.Storage;

namespace WMS.Application.Interfaces
{
    public interface IUnitOfWorks
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
