namespace WMS.Application.Features.GoodsReceived.Dto
{
    public record GoodsReceiptModel(
        string Barcode,
        decimal Quantity,
        string UnitQuantity,
        decimal PurchasePrice
        );
}
