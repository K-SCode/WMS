using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class GoodsReceiptsItemsRepository(ApplicationDbContext context) : IGoodsReceiptsItemsRepository
    {
        public async Task AddRangeAsync(IEnumerable<GoodsReceiptsItems> items, CancellationToken ct)
        {
            await context.Set<GoodsReceiptsItems>().AddRangeAsync(items, ct);
        }
    }
}
