using Mediator;
using WMS.Application.Features.Raports.Dto;
using WMS.Core.Common;
using WMS.Core.Common.Enum;

namespace WMS.Application.Features.Raports.Query.GetMostProfitable
{
    public sealed record ListMostProfitableQuery(
        DateTime FromTime,
        DateTime ToTime,
        ReportPeriod Peroid = ReportPeriod.Total) : IQuery<Result<IReadOnlyCollection<MostProfitableDto>>>;
}
