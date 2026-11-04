namespace WMS.Application.Features.GoodsReceived.Dto
{
    public sealed record GoodsReceivedDto(
        int Id,
        string InvoiceNumber,
        string GRNNumber,
        DateTimeOffset DateOfIssue,
        string SupplierNip,
        string Status
        );
}
