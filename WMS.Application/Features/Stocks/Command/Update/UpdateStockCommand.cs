using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Command.Update
{
    public sealed record UpdateStockCommand(int Id, int LocationId, int ProductId, decimal Quantity, decimal MaxQuantity) : ICommand<Result<bool>>;
}
