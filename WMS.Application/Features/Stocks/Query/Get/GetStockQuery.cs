using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Application.Features.Stocks.Dtos;

namespace WMS.Application.Features.Stocks.Query.Get
{
    public sealed record GetStockQuery(int Id) : IQuery<StockDto?>;
}
