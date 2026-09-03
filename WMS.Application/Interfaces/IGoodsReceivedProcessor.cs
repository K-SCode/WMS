using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Commands;
using WMS.Application.Features.GoodsReceived.Dto;
using WMS.Core.Common.Enum;

namespace WMS.Application.Interfaces
{
    public interface IGoodsReceivedProcessor
    {
        Task<Dictionary<GoodsReceivedStatus, IEnumerable<GoodsReceivedDto>>> ProcessAsync(
            IEnumerable<ProductReceived> products,
            CancellationToken ct);
    }
}
