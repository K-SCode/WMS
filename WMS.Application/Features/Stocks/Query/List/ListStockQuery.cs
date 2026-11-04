using Mediator;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Query.List
{
    public sealed record ListStockQuery : IQuery<Result<IReadOnlyCollection<StockDto>>>;
}
