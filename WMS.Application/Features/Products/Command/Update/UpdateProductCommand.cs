using Mediator;
using WMS.Core.Common;

namespace WMS.Application.Features.Products.Command.Update
{
    public sealed record UpdateProductCommand(
        int Id,
        string Name,
        decimal Price,
        string UnitQuantity) : ICommand<Result<bool>>;
}
