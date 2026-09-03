using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Data;
using WMS.Infrastructure.Data.Connections;
using WMS.Infrastructure.Import;
using WMS.Infrastructure.Import.Adapters;
using WMS.Infrastructure.Repositories;

namespace WMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWorks, UnitOfWorks>();
            services.AddSingleton<ISqlConnectionFactory>(new SqlConnectionFactory(connectionString!));

            services.AddScoped<IProductImportFileAdapter, CsvProductAdapter>();
            services.AddScoped<IProductImportFileAdapter, XmlProductAdapter>();
            services.AddScoped<IProductImportFileAdapterFactory, ProductImportFileFactory>();

            return services;
        }
    }
}
