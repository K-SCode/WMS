namespace WMS.Application.Features.GoodsReceived.Dto
{
    public sealed record GoodsReceivedItemsDto(
            string Barcode,
            decimal Quantity,
            string UnitQuantity,
            decimal PurchasePrice
        );
}
