using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Dto
{
    public sealed record GoodsReceivedDto(string Barcode, GoodsReceivedStatus Status, int Quantity);
}
