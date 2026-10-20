
namespace WMS.Application.Features.GoodsReceived.Services
{
    public record GoodsReceiptModel(
        string Barcode,
        decimal Quantity,
        string UnitQuantity,
        decimal PurchasePrice
        );
}
