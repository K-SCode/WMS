using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Stock> Stocks { get; set; }
    }
}
