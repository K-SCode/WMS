using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public sealed record CreateStockCommand(int LocationId, int ProductId, decimal Quantity, decimal MaxQuantity) : ICommand<Result<int>>;
}
