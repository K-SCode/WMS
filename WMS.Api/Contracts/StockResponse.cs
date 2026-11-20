namespace WMS.Api.Contracts
{
    public sealed record StockResponse(int Id, int LocationId, int ProductId, decimal Quantity, int MaxQuantity);
}
