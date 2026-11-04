
namespace WMS.Application.Features.GoodsReceived.Dto
{
    public sealed record GoodsReceivedDetailsDto(
        int Id,
        string InvoiceNumber,
        string GRNNumber,
        DateTimeOffset DateOfIssue,
        DateTimeOffset ReceivedAtUtc,
        string SupplierNip,
        string Status,
        string Comment,
        IReadOnlyCollection<GoodsReceivedItemsDto> Items
        );
}
