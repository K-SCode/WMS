using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;

namespace WMS.Application.Features.Stocks.Query.List
{
    public sealed record ListStockQuery : IQuery<IEnumerable<StockDto>?>;
}
