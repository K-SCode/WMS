using WMS.Core.Common.Enum;

namespace WMS.Api.Contracts
{
    public sealed record MostProfitableItemsRequest(
        string FromDate,
        string ToDate,
        ReportPeriod ReportPeriod);
}
