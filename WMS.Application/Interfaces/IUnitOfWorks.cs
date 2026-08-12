using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IUnitOfWorks
    {
        ILocationRepository Location { get; }
        IProductRepository Product { get; }
        IStockRepository Stock { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
