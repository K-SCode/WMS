using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public sealed record CreateStockCommand(int LocationId, int ProductId, int Quantity) : ICommand<int>;
}
