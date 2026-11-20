using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WMS.Application.common.Exceptions;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Application.Interfaces.Repositories;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWork
    {
        private ILocationRepository? _location;
        private IProductRepository? _product;
        private IStockRepository? _stock;
        private IGoodsReceiptRepository? _goodsReceipt;
        private ISupplierRepository? _supplier;
        private IGoodsReceiptsItemsRepository? _goodsReceiptItems;
        private IOrderRepository? _order;
        private ICustomerRepository? _customer;
        private IOrderItemsRepository? _orderItems;
        private IWarehouseRepository? _warehouse;
        public ILocationRepository Location =>
            _location ??= new LocationRepository(context);
        public IProductRepository Product =>
            _product ??= new ProductRepository(context);
        public IStockRepository Stock =>
            _stock ??= new StockRepository(context);
        public IGoodsReceiptRepository GoodsReceipt =>
            _goodsReceipt ??= new GoodsReceiptRepository(context);
        public ISupplierRepository Supplier => 
            _supplier ??= new SupplierRespository(context);
        public IGoodsReceiptsItemsRepository GoodsReceiptsItems =>
            _goodsReceiptItems ??= new GoodsReceiptsItemsRepository(context);
        public IOrderRepository Order =>
            _order ??= new OrderRepository(context);
        public ICustomerRepository Customer =>
            _customer ??= new CustomerRepository(context);
        public IOrderItemsRepository OrderItems =>
            _orderItems ??= new OrderItemsRepository(context);
        public IWarehouseRepository Warehouse =>
            _warehouse ??= new WarehouseRepository(context);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
            context.Database.BeginTransactionAsync(cancellationToken);

        public void ClearTrackedChanges()
        {
            context.ChangeTracker.Clear();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var entities = string.Join(", ",
                    ex.Entries.Select(e => e.Metadata.ClrType.Name).Distinct());

                throw new ConcurrencyConflictException(
                    $"Concurrency conflict while saving: {entities}.", ex);
            }
        }
    }
}
