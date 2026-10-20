using WMS.Core.Common.Enum;
using static WMS.Application.common.Service.IncreaseQunatity;

namespace WMS.Application.common.Service
{
    public sealed record StockIncreaseResult(
        string Barcode,
        IReadOnlyCollection<ResolveStatus> ResolveStatuses
        );
}
