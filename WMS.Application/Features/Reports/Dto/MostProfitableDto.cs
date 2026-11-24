namespace WMS.Application.Features.Raports.Dto
{
    public sealed record MostProfitableDto(
        DateOnly PeriodStart,
        int ProductId,
        string Name,
        string Currency,
        decimal Quantity,
        decimal Revenue,
        int OrdersCount);
}
