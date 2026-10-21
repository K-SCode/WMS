using WMS.Core.Common.Enum;

namespace WMS.Application.common.Service
{
    public sealed record ResolveStatus(int? LocationId, StockIncreaseStatus Status, decimal Quantity);
}
