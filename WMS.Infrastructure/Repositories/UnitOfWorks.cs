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
        public IRepository<Location> Location => throw new NotImplementedException();

        public IRepository<Product> Product => throw new NotImplementedException();

        public IRepository<Stock> Stock => throw new NotImplementedException();

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
