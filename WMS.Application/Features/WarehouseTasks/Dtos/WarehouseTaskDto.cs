namespace WMS.Application.Features.WarehouseTasks.Dtos
{
    public sealed record WarehouseTaskDto(
        int Id,
        string Barcode,
        decimal Quantity,
        string Source,
        string Status,
        int? OrderId,
        int? GoodsReceiptId,
        int? LocationId,
        DateTimeOffset CreatedAt,
        DateTimeOffset? AssignedAt);
}
