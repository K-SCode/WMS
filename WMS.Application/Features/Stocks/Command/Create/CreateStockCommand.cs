using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Stocks.Command.Create
{
    public sealed record CreateStockCommand(int LocationId, int ProductId, int Quantity, int? MaxQuantity) : ICommand<Result<int>>;
}
