namespace WMS.Application.Features.Reports.Dto
{
    public sealed record ListStockReportDto(int ProductId,
    string Name,
    decimal Quantity,
    string Unit,
    DateTime? LastSoldAt);
}
