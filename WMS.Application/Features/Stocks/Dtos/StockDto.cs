namespace WMS.Application.Features.Stocks.Dtos
{
    public sealed record StockDto(int Id, int LocationId, int ProductId, decimal Quantity, decimal MaxQuantity);
}
