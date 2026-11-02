using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common;
using WMS.Infrastructure.Data.Identity;

namespace WMS.Infrastructure.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider service)
        {
            await using var scope = service.CreateAsyncScope();
            var serviceProvicder = scope.ServiceProvider;
            
            var database = serviceProvicder.GetRequiredService<ApplicationDbContext>();
            await database.Database.MigrateAsync();
            var roleManager = serviceProvicder.GetRequiredService<RoleManager<IdentityRole<int>>>();
            foreach (var role in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
        public static async Task SeedUserAsync(IServiceProvider service)
        {
            await using var scope = service.CreateAsyncScope();
            var serviceProvicder = scope.ServiceProvider;

            var database = serviceProvicder.GetRequiredService<ApplicationDbContext>();
            await database.Database.MigrateAsync();

            var userManager = serviceProvicder.GetRequiredService<UserManager<ApplicationUser>>();
            await SaveUserAsync(userManager, "Manager@wms.test", "Julie", "Forest", Roles.Manager);
            await SaveUserAsync(userManager, "Analyst@wms.test", "John", "Milk", Roles.Analyst);
            await SaveUserAsync(userManager, "Warehouseman@wms.test", "Jacoob", "Wood", Roles.Warehouseman);
        }

        private static async Task SaveUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string firstName,
            string lastName,
            string role)
        {
            if (await userManager.FindByEmailAsync(email) is not null)
                return;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = "123123123"
            };

            var result = await userManager.CreateAsync(user, "Qazwsxedc1!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, role);
        }
    }
}
