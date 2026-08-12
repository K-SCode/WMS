using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Query.Get
{
    public sealed record GetStockQuery(int Id) : IQuery<Result<StockDto>>;
}
