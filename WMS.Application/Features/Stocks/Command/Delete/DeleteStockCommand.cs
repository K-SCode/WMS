using Mediator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WMS.Application.Features.Stocks.Command.Delete
{
    public sealed record DeleteStockCommand(int Id) : ICommand<bool>;
}
