
using Microsoft.AspNetCore.Identity;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;
using WMS.Infrastructure.Data.Identity;

namespace WMS.Infrastructure.Auth
{
    public class WorkerDirectory(UserManager<ApplicationUser> userManager)
        : IWorkerDirectory
    {
        public async Task<IReadOnlyCollection<int>> GetActiveWarehousemenIdsAsync(CancellationToken ct)
        {
            var warehouseman = await userManager.GetUsersInRoleAsync(Roles.Warehouseman);

            return warehouseman
                .Where(user => user.IsActive)
                .Select(user => user.Id)
                .ToList();
        }
    }
}
