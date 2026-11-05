using WMS.Core.Common.Enum;

namespace WMS.Application.common.Service
{
    public sealed record ResolveStatus(int? LocationId, StockManageStatus Status, decimal Quantity);
}
