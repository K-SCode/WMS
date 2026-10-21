namespace WMS.Application.common.Service
{
    public sealed record StockResult(
        string Barcode,
        IReadOnlyCollection<ResolveStatus> ResolveStatuses
        );
}
