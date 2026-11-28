using Mediator;
using WMS.Application.Features.Reports.Dto;
using WMS.Core.Common;

namespace WMS.Application.Features.Reports.Query.ListStockReport
{
    public sealed record ListStockReportQuery : IQuery<Result<IDictionary<string, List<ListStockReportDto>>>>;
}
