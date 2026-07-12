using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IUnitOfWorks
    {
        IRepository<Location> Location { get; }
        IRepository<Product> Product { get; }
        IRepository<Stock> Stock { get; }
        Task SaveChangesAsync();
    }
}
