using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Interfaces
{
    public interface IGoodsReceiptRepository
    {
        Task AddAsync(GoodsReceipt goodsReceipt, CancellationToken cancellationToken);
    }
}
