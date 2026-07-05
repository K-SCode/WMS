using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Product {  get; set; }
        public DbSet<Localization> Localization { get; set; }
        public DbSet<Stock> Stock { get; set;  }
        //TO DO:
        //CREATE FLUENT API
        //CHECK HOW I SHOULD CREATE A DBCONTEX
        //ADD MIGRATIONS
        //ADD REPOSITORY
        //ADD SERVICES
        //ADD FLUENTVALIDATION
        //ADD MAPPER
        //ADD 
    }
}
