using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWorks
    {
        private ILocationRepository? _location;
        private IProductRepository? _product;
        private IStockRepository? _stock;


        public ILocationRepository Location =>
            _location ??= new LocationRepository(context);
        public IProductRepository Product =>
            _product ??= new ProductRepository(context);
        public IStockRepository Stock =>
            _stock ??= new StockRepository(context);


        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await context.SaveChangesAsync(cancellationToken);
    }
}
