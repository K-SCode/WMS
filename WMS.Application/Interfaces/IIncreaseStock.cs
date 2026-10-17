using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Application.Features.GoodsReceived.Services;
using WMS.Core.Common.Enum;

namespace WMS.Application.Interfaces
{
    public interface IIncreaseStock
    {
        Task<Dictionary<ItemGrnStatus, IEnumerable<GoodsReceiptModel>>> ProcessAsync(
            IEnumerable<ProductReceived> products,
            CancellationToken ct);
    }
}
