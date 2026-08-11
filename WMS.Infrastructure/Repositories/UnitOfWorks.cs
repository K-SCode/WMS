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
        private IRepository<Stock>? _stock;


        public ILocationRepository Location =>
            _location ??= new LocationRepository(context);
        public IProductRepository Product =>
            _product ??= new ProductRepository(context);
        public IRepository<Stock> Stock =>
            _stock ??= new Repository<Stock>(context);


        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await context.SaveChangesAsync(cancellationToken);
    }
}
