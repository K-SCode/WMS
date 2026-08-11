using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class LocationRepository(ApplicationDbContext context) : Repository<Location>(context), ILocationRepository
    {
        public async Task<bool> LocationExistsAsync(char sector, int rack, int number,CancellationToken cancellationToken)
        {

            return await _dbSet.AnyAsync(location =>
                location.Sector == sector &&
                location.Rack == rack &&
                location.Number == number,
                cancellationToken);
        }
    }
}
