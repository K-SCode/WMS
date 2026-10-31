namespace WMS.Application.Features.GoodsReceived.Dto
{
    public sealed record GoodsReceivedDto(
        int Id,
        string InvoiceNumber,
        string GRNNumber,
        DateTime DateOfIssue,
        string SupplierNip,
        string Status
        );
}
