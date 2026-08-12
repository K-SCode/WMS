using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface ILocationRepository : IStockRepository<Location>
    {
        Task<bool> LocationExistsAsync(char sector, int rack, int number, CancellationToken cancellationToken);
    }
}
