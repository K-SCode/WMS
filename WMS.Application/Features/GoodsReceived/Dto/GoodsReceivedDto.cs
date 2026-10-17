using WMS.Core.Common.Enum;

namespace WMS.Application.Features.GoodsReceived.Dto
{
    public sealed record GoodsReceivedDto(string Barcode, decimal Quantity);
}
