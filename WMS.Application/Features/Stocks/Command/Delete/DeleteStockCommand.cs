using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Command.Delete
{
    public sealed record DeleteStockCommand(int Id) : ICommand<Result<bool>>;
}
