using WMS.Application.Interfaces;
using WMS.Core.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class GoodsReceiptRepository(ApplicationDbContext context) : IGoodsReceiptRepository
    {
        public async Task AddAsync(GoodsReceipt goodsReceipt, CancellationToken cancellationToken)
        {
            await context.Set<GoodsReceipt>().AddAsync(goodsReceipt, cancellationToken);
        }
    }
}
