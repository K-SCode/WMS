using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public sealed record UpdateStockCommand(int Id, int LocationId, int ProductId, int Quantity) : ICommand<Result<bool>>;
}
