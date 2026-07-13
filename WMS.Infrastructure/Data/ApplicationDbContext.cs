using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : DbContext(options), IAppDbContext
    {
        public DbSet<Product> Products {  get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Stock> Stocks { get; set;  }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
