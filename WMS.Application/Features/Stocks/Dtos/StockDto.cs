using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.Features.Stocks.Dtos
{
    public sealed record StockDto(int Id, int LocationId, int ProductId, int Quantity);
}
