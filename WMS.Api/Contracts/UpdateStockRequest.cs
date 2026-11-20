namespace WMS.Api.Contracts
{
    public sealed record UpdateStockRequest(int LocationId, int ProductId, decimal Quantity, decimal MaxQuantity);
}
