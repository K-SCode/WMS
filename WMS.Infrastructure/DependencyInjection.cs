using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Infrastructure.Auth;
using WMS.Infrastructure.Data;
using WMS.Infrastructure.Data.Connections;
using WMS.Infrastructure.Data.Identity;
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
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
                ?? throw new InvalidOperationException($"{nameof(JwtSettings)} not exists");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = JwtRegisteredClaimNames.Name,
                    RoleClaimType = "role"
                };
            });

            services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.Password.RequiredLength = 10;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.ManagerOrWarehouseman,
                    policy => policy.RequireRole(Roles.Warehouseman, Roles.Manager));
            });

            return services;
        }
    }
}
