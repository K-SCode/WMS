using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;
using WMS.Core.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWorks
    {
        private IRepository<Location>? _location;
        private IRepository<Product>? _product;
        private IRepository<Stock>? _stock;


        public IRepository<Location> Location =>
            _location ??= new Repository<Location>(context);
        public IRepository<Product> Product =>
            _product ??= new Repository<Product>(context);
        public IRepository<Stock> Stock =>
            _stock ??= new Repository<Stock>(context);


        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();
    }
}
