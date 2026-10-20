using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.GoodsReceived.Commands;

namespace WMS.Application.Features.GoodsReceived.Dto
{
    public record GoodsReceivedNote(IReadOnlyCollection<ProductReceived> Products, DocumentInformation Information);
}
