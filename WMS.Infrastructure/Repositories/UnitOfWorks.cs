using WMS.Application.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWorks
    {
        private ILocationRepository? _location;
        private IProductRepository? _product;
        private IStockRepository? _stock;
        private IGoodsReceiptRepository? _goodsReceipt;
        private ISupplierRepository? _supplier;
        private IGoodsReceiptsItemsRepository? _goodsReceiptItems;
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


        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await context.SaveChangesAsync(cancellationToken);
    }
}
