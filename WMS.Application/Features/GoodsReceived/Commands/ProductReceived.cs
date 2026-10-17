namespace WMS.Application.Features.GoodsReceived.Commands
{
    public sealed record ProductReceived(string Barcode, decimal Quantity,string UnitQuantity, decimal PurchasePrice);
}
